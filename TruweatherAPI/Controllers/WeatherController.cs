using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using TruweatherCore.Models.DTOs;
using TruweatherCore.Services.Interfaces;

namespace TruweatherAPI.Controllers;

/// <summary>
/// API endpoints for weather data, location management, and alert configuration.
///
/// Provides real-time weather from Open-Meteo API (free, no API key), 7-day forecasts,
/// saved location CRUD, and weather alert setup. Includes response caching (60 seconds)
/// and in-memory data cache (60 minutes) with database fallback on API failure.
///
/// All endpoints require JWT authentication via Authorization header.
/// Rate limit: 100 requests per minute per IP address.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[EnableRateLimiting("api")]
public class WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger) : ControllerBase
{
    private readonly IWeatherService _weatherService = weatherService;
    private readonly ILogger<WeatherController> _logger = logger;

    private string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException();
    }

    /// <summary>Get current weather conditions for specified latitude and longitude.</summary>
    /// <remarks>
    /// Returns real-time weather data from Open-Meteo API including temperature, feels-like,
    /// humidity, wind speed/direction, pressure, visibility, and condition description.
    ///
    /// **Caching**: Response cached by browser for 60 seconds. Server-side cached for 60 minutes.
    /// X-Data-Source header indicates source: "Live" (API), "MemoryCache", "Database" (fallback), or "Fallback" (mock).
    ///
    /// **Coordinates**: Use decimal format. Queries to saved locations may be cached longer.
    /// </remarks>
    /// <param name="latitude">Latitude in decimal degrees (-90 to 90). Examples: 40.7128, -33.8688</param>
    /// <param name="longitude">Longitude in decimal degrees (-180 to 180). Examples: -74.0060, 151.2093</param>
    /// <response code="200">Current weather data with temperature, conditions, and forecast metadata</response>
    /// <response code="401">Unauthorized - Valid JWT access token required in Authorization header</response>
    /// <response code="404">Weather data unavailable (typically no Open-Meteo coverage for coordinates)</response>
    /// <response code="429">Rate limit exceeded (100 requests/minute)</response>
    [HttpGet("current")]
    [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "latitude", "longitude" })]
    [ProducesResponseType(typeof(CurrentWeatherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<CurrentWeatherDto>> GetCurrentWeather(
        [FromQuery] decimal latitude,
        [FromQuery] decimal longitude)
    {
        var weather = await _weatherService.GetCurrentWeatherAsync(latitude, longitude);
        return weather == null ? NotFound() : Ok(weather);
    }

    /// <summary>Get 7-day weather forecast for specified latitude and longitude.</summary>
    /// <remarks>
    /// Returns 7-day weather forecast from Open-Meteo API with daily high/low/average temperatures,
    /// condition descriptions, humidity, wind speed, precipitation, and weather icons.
    ///
    /// **Caching**: Response cached by browser for 60 seconds. Server-side cached for 60 minutes.
    /// X-Data-Source header indicates source: "Live" (API), "MemoryCache", or "Fallback" (mock data).
    ///
    /// **Forecast Accuracy**: Updates daily. Accuracy decreases 5-7 days out.
    /// </remarks>
    /// <param name="latitude">Latitude in decimal degrees (-90 to 90)</param>
    /// <param name="longitude">Longitude in decimal degrees (-180 to 180)</param>
    /// <response code="200">7-day forecast with daily conditions, temperatures, and precipitation</response>
    /// <response code="401">Unauthorized - Valid JWT access token required</response>
    /// <response code="404">Forecast data unavailable (typically no Open-Meteo coverage)</response>
    /// <response code="429">Rate limit exceeded (100 requests/minute)</response>
    [HttpGet("forecast")]
    [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "latitude", "longitude" })]
    [ProducesResponseType(typeof(ForecastDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<ForecastDto>> GetForecast(
        [FromQuery] decimal latitude,
        [FromQuery] decimal longitude)
    {
        var forecast = await _weatherService.GetForecastAsync(latitude, longitude);
        return forecast == null ? NotFound() : Ok(forecast);
    }

    /// <summary>Get all saved locations for the authenticated user.</summary>
    /// <remarks>
    /// Returns all locations saved by the user, including name, coordinates, and default flag.
    /// Each location has a unique ID used for managing alerts and fetching location-specific weather.
    /// Default location is the initial location shown when user logs in.
    /// </remarks>
    /// <response code="200">Array of saved locations with IDs, names, coordinates, and default status</response>
    /// <response code="401">Unauthorized - Valid JWT access token required</response>
    /// <response code="429">Rate limit exceeded (100 requests/minute)</response>
    [HttpGet("locations")]
    [ProducesResponseType(typeof(List<SavedLocationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<List<SavedLocationDto>>> GetSavedLocations()
    {
        var userId = GetUserId();
        var locations = await _weatherService.GetSavedLocationsAsync(userId);
        return Ok(locations);
    }

    /// <summary>Add a new saved location for the authenticated user.</summary>
    /// <remarks>
    /// Creates a new saved location with name and coordinates. User can mark as default.
    /// Only one location can be default; setting a new default updates the previous one.
    /// Location ID is returned in response and used for weather queries and alert setup.
    /// </remarks>
    /// <param name="request">Location details (name, latitude, longitude, isDefault)</param>
    /// <response code="201">Location created successfully, returns location with auto-generated ID</response>
    /// <response code="400">Validation error (invalid coordinates, empty name, etc)</response>
    /// <response code="401">Unauthorized - Valid JWT access token required</response>
    /// <response code="429">Rate limit exceeded (100 requests/minute)</response>
    [HttpPost("locations")]
    [ProducesResponseType(typeof(SavedLocationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<SavedLocationDto>> AddSavedLocation(
        CreateLocationRequest request)
    {
        var userId = GetUserId();
        var location = await _weatherService.AddSavedLocationAsync(userId, request);
        return Created(nameof(GetSavedLocations), location);
    }

    /// <summary>Update an existing saved location by ID.</summary>
    /// <remarks>
    /// Allows updating location name and default status. Coordinates are read-only.
    /// User can only update their own locations (enforced via UserId claim).
    /// Only one location can be default at a time.
    /// </remarks>
    /// <param name="id">Location ID to update (must belong to authenticated user)</param>
    /// <param name="request">Updated details (name, isDefault). Omit to keep unchanged.</param>
    /// <response code="204">Location updated successfully</response>
    /// <response code="400">Validation error (invalid name, etc)</response>
    /// <response code="401">Unauthorized - Valid JWT access token required</response>
    /// <response code="404">Location not found or does not belong to user</response>
    /// <response code="429">Rate limit exceeded (100 requests/minute)</response>
    [HttpPut("locations/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> UpdateSavedLocation(
        int id,
        UpdateLocationRequest request)
    {
        var userId = GetUserId();
        var success = await _weatherService.UpdateSavedLocationAsync(userId, id, request);
        return success ? NoContent() : NotFound();
    }

    /// <summary>Delete a saved location by ID.</summary>
    /// <remarks>
    /// Removes location and all associated weather alerts. User can only delete their own locations.
    /// Cannot delete if it's the last location; at least one location required.
    /// If deleted location was default, no new default is auto-assigned.
    /// </remarks>
    /// <param name="id">Location ID to delete (must belong to authenticated user)</param>
    /// <response code="204">Location deleted successfully, with all associated alerts</response>
    /// <response code="401">Unauthorized - Valid JWT access token required</response>
    /// <response code="404">Location not found or does not belong to user</response>
    /// <response code="429">Rate limit exceeded (100 requests/minute)</response>
    [HttpDelete("locations/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> DeleteSavedLocation(int id)
    {
        var userId = GetUserId();
        var success = await _weatherService.DeleteSavedLocationAsync(userId, id);
        return success ? NoContent() : NotFound();
    }

    /// <summary>Get all weather alerts configured by the authenticated user.</summary>
    /// <remarks>
    /// Returns alerts with trigger conditions (e.g., temp greater than 90, humidity less than 30%).
    /// Each alert linked to a specific saved location and alert type (temperature, wind, etc).
    /// IsEnabled indicates whether alert is active.
    /// </remarks>
    /// <response code="200">Array of alerts with location ID, type, condition, threshold, and enabled status</response>
    /// <response code="401">Unauthorized - Valid JWT access token required</response>
    /// <response code="429">Rate limit exceeded (100 requests/minute)</response>
    [HttpGet("alerts")]
    [ProducesResponseType(typeof(List<WeatherAlertDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<List<WeatherAlertDto>>> GetWeatherAlerts()
    {
        var userId = GetUserId();
        var alerts = await _weatherService.GetWeatherAlertsAsync(userId);
        return Ok(alerts);
    }

    /// <summary>Create a new weather alert for a saved location.</summary>
    /// <remarks>
    /// Triggers notifications when weather condition is met (e.g., temp greater than threshold).
    /// AlertType: "temperature", "wind", "precipitation", "humidity", "pressure".
    /// Condition: greater than, less than, equals, etc.
    /// Example: AlertType=temperature, Condition=greater than, Threshold=90 for alerts when temp exceeds 90.
    /// </remarks>
    /// <param name="request">Alert configuration (savedLocationId, alertType, condition, threshold)</param>
    /// <response code="201">Alert created successfully, returns alert with auto-generated ID</response>
    /// <response code="400">Validation error (invalid location ID, threshold format, etc)</response>
    /// <response code="401">Unauthorized - Valid JWT access token required</response>
    /// <response code="429">Rate limit exceeded (100 requests/minute)</response>
    [HttpPost("alerts")]
    [ProducesResponseType(typeof(WeatherAlertDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<WeatherAlertDto>> CreateWeatherAlert(
        CreateWeatherAlertRequest request)
    {
        var userId = GetUserId();
        var alert = await _weatherService.CreateWeatherAlertAsync(userId, request);
        return Created(nameof(GetWeatherAlerts), alert);
    }

    /// <summary>Update an existing weather alert by ID.</summary>
    /// <remarks>
    /// Allows modifying alert type, condition, threshold, and enabled status.
    /// User can only update their own alerts (enforced via UserId claim).
    /// </remarks>
    /// <param name="id">Alert ID to update (must belong to authenticated user)</param>
    /// <param name="request">Updated alert configuration (type, condition, threshold, isEnabled)</param>
    /// <response code="204">Alert updated successfully</response>
    /// <response code="400">Validation error in request</response>
    /// <response code="401">Unauthorized - Valid JWT access token required</response>
    /// <response code="404">Alert not found or does not belong to user</response>
    /// <response code="429">Rate limit exceeded (100 requests/minute)</response>
    [HttpPut("alerts/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> UpdateWeatherAlert(
        int id,
        UpdateWeatherAlertRequest request)
    {
        var userId = GetUserId();
        var success = await _weatherService.UpdateWeatherAlertAsync(userId, id, request);
        return success ? NoContent() : NotFound();
    }

    /// <summary>Delete a weather alert by ID.</summary>
    /// <remarks>
    /// Removes the alert permanently. User can only delete their own alerts.
    /// Deleting does not affect the associated saved location.
    /// </remarks>
    /// <param name="id">Alert ID to delete (must belong to authenticated user)</param>
    /// <response code="204">Alert deleted successfully</response>
    /// <response code="401">Unauthorized - Valid JWT access token required</response>
    /// <response code="404">Alert not found or does not belong to user</response>
    /// <response code="429">Rate limit exceeded (100 requests/minute)</response>
    [HttpDelete("alerts/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> DeleteWeatherAlert(int id)
    {
        var userId = GetUserId();
        var success = await _weatherService.DeleteWeatherAlertAsync(userId, id);
        return success ? NoContent() : NotFound();
    }
}

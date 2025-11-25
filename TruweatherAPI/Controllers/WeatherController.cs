using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TruweatherAPI.DTOs;
using TruweatherAPI.Services;

namespace TruweatherAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WeatherController(IWeatherService weatherService) : ControllerBase
{
    private readonly IWeatherService _weatherService = weatherService;

    private string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException();
    }

    [HttpGet("current")]
    public async Task<ActionResult<CurrentWeatherDto>> GetCurrentWeather(
        [FromQuery] decimal latitude,
        [FromQuery] decimal longitude)
    {
        var weather = await _weatherService.GetCurrentWeatherAsync(latitude, longitude);
        return weather == null ? NotFound() : Ok(weather);
    }

    [HttpGet("forecast")]
    public async Task<ActionResult<ForecastDto>> GetForecast(
        [FromQuery] decimal latitude,
        [FromQuery] decimal longitude)
    {
        var forecast = await _weatherService.GetForecastAsync(latitude, longitude);
        return forecast == null ? NotFound() : Ok(forecast);
    }

    [HttpGet("locations")]
    public async Task<ActionResult<List<SavedLocationDto>>> GetSavedLocations()
    {
        var userId = GetUserId();
        var locations = await _weatherService.GetSavedLocationsAsync(userId);
        return Ok(locations);
    }

    [HttpPost("locations")]
    public async Task<ActionResult<SavedLocationDto>> AddSavedLocation(
        CreateLocationRequest request)
    {
        var userId = GetUserId();
        var location = await _weatherService.AddSavedLocationAsync(userId, request);
        return Created(nameof(GetSavedLocations), location);
    }

    [HttpPut("locations/{id}")]
    public async Task<IActionResult> UpdateSavedLocation(
        int id,
        UpdateLocationRequest request)
    {
        var userId = GetUserId();
        var success = await _weatherService.UpdateSavedLocationAsync(userId, id, request);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("locations/{id}")]
    public async Task<IActionResult> DeleteSavedLocation(int id)
    {
        var userId = GetUserId();
        var success = await _weatherService.DeleteSavedLocationAsync(userId, id);
        return success ? NoContent() : NotFound();
    }

    [HttpGet("alerts")]
    public async Task<ActionResult<List<WeatherAlertDto>>> GetWeatherAlerts()
    {
        var userId = GetUserId();
        var alerts = await _weatherService.GetWeatherAlertsAsync(userId);
        return Ok(alerts);
    }

    [HttpPost("alerts")]
    public async Task<ActionResult<WeatherAlertDto>> CreateWeatherAlert(
        CreateWeatherAlertRequest request)
    {
        var userId = GetUserId();
        var alert = await _weatherService.CreateWeatherAlertAsync(userId, request);
        return Created(nameof(GetWeatherAlerts), alert);
    }

    [HttpPut("alerts/{id}")]
    public async Task<IActionResult> UpdateWeatherAlert(
        int id,
        UpdateWeatherAlertRequest request)
    {
        var userId = GetUserId();
        var success = await _weatherService.UpdateWeatherAlertAsync(userId, id, request);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("alerts/{id}")]
    public async Task<IActionResult> DeleteWeatherAlert(int id)
    {
        var userId = GetUserId();
        var success = await _weatherService.DeleteWeatherAlertAsync(userId, id);
        return success ? NoContent() : NotFound();
    }
}

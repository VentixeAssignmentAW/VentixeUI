using System.Net.Http.Json;

namespace VentixeUI.Services;

public interface IBookingService
{
    Task<List<BookingDto>> GetBookingsForEventAsync(Guid eventId);
    Task<BookingDto?> CreateBookingAsync(CreateBookingRequest request);
    Task<bool> CancelBookingAsync(Guid bookingId);
}

public class BookingService : IBookingService
{
    private readonly HttpClient _httpClient;

    public BookingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<BookingDto>> GetBookingsForEventAsync(Guid eventId)
    {
        try
        {
            var bookings = await _httpClient.GetFromJsonAsync<List<BookingDto>>($"api/booking/event/{eventId}");
            return bookings ?? new List<BookingDto>();
        }
        catch(Exception ex)
        {
            return new List<BookingDto>();
        }
    }

    public async Task<BookingDto?> CreateBookingAsync(CreateBookingRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/booking", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<BookingDto>();
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CancelBookingAsync(Guid bookingId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/booking/{bookingId}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}

public class BookingDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public Guid EventId { get; set; }
    public DateTime BookingDate { get; set; }
    public BookingStatus Status { get; set; }
    public decimal Price { get; set; }
}

public enum BookingStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Paid
}

public class CreateBookingRequest
{
    public string Email { get; set; } = string.Empty;
    public Guid EventId { get; set; }
    public decimal Price { get; set; }
} 
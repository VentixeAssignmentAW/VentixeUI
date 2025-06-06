using System.Net.Http.Json;

namespace VentixeUI.Services;

public interface IEventService
{
    Task<List<EventDto>> GetAllEventsAsync();
    Task<EventDto?> GetEventByIdAsync(Guid id);
}

public class EventService : IEventService
{
    private readonly HttpClient _httpClient;

    public EventService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EventDto>> GetAllEventsAsync()
    {
        try
        {
            var events = await _httpClient.GetFromJsonAsync<List<EventDto>>("api/events");
            return events ?? new List<EventDto>();
        }
        catch(Exception x)
        {
            Console.WriteLine($"Error fetching events: {x.Message}");
            return new List<EventDto>();
        }
    }

    public async Task<EventDto?> GetEventByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<EventDto>($"api/events/{id}");
        }
        catch(Exception x)
        {
            Console.WriteLine($"Error fetching event {id}: {x.Message}");
            return null;
        }
    }
}

public class EventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
} 
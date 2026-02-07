using Microsoft.AspNetCore.Components;
using WeatherApp.Helpers.IHelpers;
using WeatherApp.Models;
using WeatherApp.Services.IServices;

namespace WeatherApp.Components.Pages;

public partial class WeatherFight
{
    [Inject] public IWeatherService WeatherService { get; set; } = default!;
    [Inject] public IWeatherHelper WeatherHelper { get; set; } = default!;
    [Inject] public IWeatherScoreHelper ScoreHelper { get; set; } = default!;
    [Inject] public IConfiguration Configuration { get; set; } = default!;

    public string CityNameA { get; set; } = string.Empty;
    public string CityNameB { get; set; } = string.Empty;

    public WeatherViewModel? ResultA { get; private set; }
    public WeatherViewModel? ResultB { get; private set; }

    public double ScoreA { get; private set; }
    public double ScoreB { get; private set; }

    public bool IsSearching { get; private set; }
    public FightState CurrentState { get; private set; } = FightState.Idle;

    private string HeroBackgroundStyle => $"background-image: url('{Configuration["WeatherSettings:BackgroundImageUrl"]}');";

    private async Task StartFight()
    {
        if (string.IsNullOrWhiteSpace(CityNameA) || string.IsNullOrWhiteSpace(CityNameB)) return;

        IsSearching = true;
        CurrentState = FightState.Searching;
        StateHasChanged();

        try
        {
            var taskA = WeatherService.GetRawWeatherAsync(CityNameA);
            var taskB = WeatherService.GetRawWeatherAsync(CityNameB);

            await Task.WhenAll(taskA, taskB);

            if (taskA.Result != null && taskB.Result != null)
            {
                ResultA = WeatherHelper.MapToViewModel(taskA.Result);
                ResultB = WeatherHelper.MapToViewModel(taskB.Result);

                ScoreA = ScoreHelper.CalculateScore(taskA.Result);
                ScoreB = ScoreHelper.CalculateScore(taskB.Result);

                CurrentState = FightState.Success;
            }
            else
            {
                CurrentState = FightState.Error;
            }
        }
        finally
        {
            IsSearching = false;
            StateHasChanged();
        }
    }

    private string GetFighterClass(double myScore, double opponentScore)
    {
        if (Math.Abs(myScore - opponentScore) < 0.1) return "draw";
        return myScore > opponentScore ? "winner" : "loser";
    }

    public enum FightState { Idle, Searching, Success, Error }
}
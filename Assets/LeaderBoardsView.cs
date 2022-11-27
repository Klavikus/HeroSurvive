using System.Collections;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Models;
using CodeBase.MVVM.Views;
using UnityEngine;

public class LeaderBoardsView : MonoBehaviour
{
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private Transform _scoreViewsContainer;

    private List<LeaderBoardScoreView> _leaderBoardScoreViews;
    private LeaderBoardsViewModel _leaderBoardsViewModel;
    private IViewFactory _viewFactory;

    private void Start()
    {
        Show();
        _leaderBoardsViewModel = AllServices.Container.Single<IViewModelProvider>().LeaderBoardsViewModel;
        _viewFactory = AllServices.Container.Single<IViewFactory>();
        _leaderBoardsViewModel.LeaderBoardUpdated += CreateScoreViews;
        CreateScoreViews(_leaderBoardsViewModel.EntriesData);
        StartCoroutine(UpdateView());
    }

    private IEnumerator UpdateView()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("UpdateView");
        CreateScoreViews(_leaderBoardsViewModel.EntriesData);
    }

    private void CreateScoreViews(IReadOnlyList<LeaderBoardEntryData> _entriesData)
    {
        Debug.Log($"{nameof(CreateScoreViews)}");
        if (_entriesData == null)
            return;
        Debug.Log($"{_entriesData}");

        foreach (LeaderBoardScoreView leaderBoardScoreView in _leaderBoardScoreViews)
            Destroy(leaderBoardScoreView.gameObject);

        _leaderBoardScoreViews.Clear();

        foreach (LeaderBoardEntryData entryData in _entriesData)
        {
            Debug.Log($"foreach (LeaderBoardEntryData entryData in _entriesData)");
            Debug.Log($"entryData {entryData}");

            Debug.Log("count: " + entryData.EntriesData.Count);
            Debug.Log(string.Join(' ', entryData.EntriesData.Keys));
            Debug.Log(string.Join(' ', entryData.EntriesData.Values));

            foreach (KeyValuePair<string, int> keyValuePair in entryData.EntriesData)
            {
                Debug.Log($"keyValuePair: k {keyValuePair.Key} v {keyValuePair.Value}");
                LeaderBoardScoreView scoreView = _viewFactory.CreateLeaderBoardScoreView();
                scoreView.Initialize(keyValuePair.Key, keyValuePair.Value);
                scoreView.transform.SetParent(_scoreViewsContainer);
                scoreView.transform.localScale = Vector3.one;
                _leaderBoardScoreViews.Add(scoreView);
            }

            Debug.Log($"{nameof(CreateScoreViews)} done");
        }
    }

    private void Show() => _mainCanvas.enabled = true;
    private void Hide() => _mainCanvas.enabled = false;
}
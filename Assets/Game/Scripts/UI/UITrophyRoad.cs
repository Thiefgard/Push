using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITrophyRoad : UIElement
{
    [System.Serializable]
    private class RoadLevelState
    {
        public GameObject _complete;
        public GameObject _current;
        public GameObject _next;
    }

    [SerializeField] private RoadLevelState _uniqueLevel;
    [SerializeField] private List<RoadLevelState> _listLevels;
    //[SerializeField] List<Level> _levelList;
    //[SerializeField] Level[] _levelsPack;

    public override void Show()
    {
        Init();

        base.Show();
    }

    private void Init()
    {
        
        int num = GameManager.Instance.LevelNum % (_listLevels.Count + 1);
        _uniqueLevel._complete.SetActive(false);
        _uniqueLevel._current.SetActive(false);

        for (int i = 0; i < _listLevels.Count; ++i)
        {
            _listLevels[i]._complete.SetActive(false);
            _listLevels[i]._current.SetActive(false);
            _listLevels[i]._next.SetActive(false);

            if (i < num)
            {
                _listLevels[i]._complete.SetActive(true);
            }
            else if (i == num)
            {
                _listLevels[i]._current.SetActive(true);
            }
            else
            {
                _listLevels[i]._next.SetActive(true);
            }
        }

        if (num == _listLevels.Count)
        {
            _uniqueLevel._current.SetActive(true);
            _uniqueLevel._next.SetActive(false);
        }
        //else
        //{
        //    _uniqueLevel.enabled = false;
        //}
    }
}

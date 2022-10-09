using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] _sections;
    [SerializeField] private Finish _finish;
    [SerializeField] private int _nextSectionIndex = 0;
    private LevelType _level;

    private void Start()
    {
        _level = Level.Instance.gameObject.GetComponent<LevelType>();
        if(_nextSectionIndex == 0)
        {
            if (!_sections[_nextSectionIndex]._isWaveSpawned) _sections[_nextSectionIndex].SpawnEnemies();
        }
        for (int i = 0; i < _sections.Length; i++)
        {
            SectionOn(i);
        }
        if(_level._levelType == LevelType.LevelT._arena)
        {
            _finish.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_level._levelType == LevelType.LevelT._base)
        {
            if (_sections[_nextSectionIndex]._isWaveSpawned)
            {
                if (_sections[_nextSectionIndex]._isWaveSpawned && !CheckPreviousSection(_sections, _nextSectionIndex))
                {
                    _sections[_nextSectionIndex - 1].PreviousWaveDestroy();
                }

                if (_nextSectionIndex < _sections.Length - 1) _nextSectionIndex += 1;

            }
        }

        if (_level._levelType == LevelType.LevelT._arena)
        {
            _finish.gameObject.SetActive(false);
            foreach (SpawnPoint section in _sections)
            {
                section._spawnTrigger.gameObject.SetActive(false);
            }
            //for(int i = _nextSectionIndex; i < _sections.Length; i++)
            //{

            //}
            if (_sections[_nextSectionIndex].IsWaveCleared() && _nextSectionIndex < _sections.Length - 1)
            {
                _nextSectionIndex += 1;
                print(_nextSectionIndex);
                _sections[_nextSectionIndex].SpawnEnemies();
            }
            if(_nextSectionIndex == _sections.Length - 1 && _sections[_nextSectionIndex].IsWaveCleared())
            {
                _finish.gameObject.SetActive(true);
            }
        }
    }
            

    bool CheckPreviousSection(SpawnPoint[] sections, int nextSection)
    {
        if (nextSection == 0) return true;
        if (sections[nextSection - 1].IsWaveCleared()) return true;
        return false;
    }
    void SectionOn(int sectionIndex)
    {
        _sections[sectionIndex].gameObject.active = true;
    }
}

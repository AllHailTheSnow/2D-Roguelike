using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>, ISaveManager
{
    private Transform player;
    private Transform playerAnim;

    [Header("Checkpoint Info")]
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closestCheckpointId;

    [Header("Currency Info")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;
    private bool pausedGame;

    private void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();

        player = PlayerManager.Instance.player.transform;
    }

    public void RestartScene()
    {
        SaveManager.Instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void PauseGame(bool _pause)
    {
        if(_pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);

            if(distanceToCheckpoint < closestDistance && checkpoint.activeStatus == true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }

    public void LoadData(GameData _data)
    {
        StartCoroutine(LoadWithDelay(_data));
    }

    private void LoadCheckpoints(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkPoints)
        {
            foreach (Checkpoint cp in checkpoints)
            {
                if (cp.id == pair.Key && pair.Value == true)
                {
                    cp.ActivateCheckpoint();
                }
            }
        }
    }

    private void LoadClosestCheckpoint(GameData _data)
    {
        if(_data.closestCheckpointID == null)
        {
            return;
        }

        closestCheckpointId = _data.closestCheckpointID;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (_data.closestCheckpointID == checkpoint.id)
            {
                player.position = checkpoint.transform.position;
            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if(lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrency>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = (player.position.y + 0.7f);

        if (FindClosestCheckpoint() != null)
        {
            _data.closestCheckpointID = FindClosestCheckpoint().id;
        }

        _data.checkPoints.Clear();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkPoints.Add(checkpoint.id, checkpoint.activeStatus);
        }
    }

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(0.1f);
        LoadCheckpoints(_data);
        LoadClosestCheckpoint(_data);
        LoadLostCurrency(_data);
    }
}

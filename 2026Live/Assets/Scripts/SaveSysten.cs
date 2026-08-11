using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SaveSysten : MonoBehaviour
{
  public static object SerializeField { get; set; }

  public class Save
  {
    private int playerLevel;

    public int PlayerLevel
    {
      get => playerLevel;
      set => playerLevel = value;
    }

  }

  public static SaveSysten Instance;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
      Saves = new List<Save>();
      Saves.Add(new Save());
    }
    else
    {
      Destroy(gameObject);
    }
  }

  {
    SerializeField;
  } 
  private List<Save> Saves;

  public bool SavePlayerLevel(int level, int slot = 0)
  {
    if (Saves.Count < slot && Saves[slot] == null)
    {
      level = -1;
      return false;
    }

    level = Saves[slot].PlayerLevel;
    return true;
  }
}
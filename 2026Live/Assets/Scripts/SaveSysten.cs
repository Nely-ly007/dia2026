using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor.Overlays;
using UnityEngine;

// criar a classe de save - done
// criar funcao para gravar os dados no save - done

// para salvar:
// converter a classe de save para um json - done
// criar um arquivo e escrever o conteudo do json
// fechar o arquivo

// para carregar:
// abrir o arquivo
// ler o conteudo em formato json e criar um objeto save
// fechar o arquivo
// garantir que o save é valido
// precisa passar o conteudo do save pros objetos de jogo



public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Saves = new List<Save>();
            Saves.Add(new Save());
            dataPath = Application.persistentDataPath + "save";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private string dataPath;
    [SerializeField] private List<Save> Saves;

    #region Save/Load PlayerLevel
    
    public bool SavePlayerLevel(int level, int slot = 0)
    {
        if (Saves.Count < slot && Saves[slot] == null) return false;
        Saves[slot].playerLevel = level;
        return true;
    }
    
    public bool LoadPlayerLevel(out int level, int slot = 0)
    {
        if (Saves.Count < slot && Saves[slot] == null)
        {
            level = -1;
            return false;
        }
        level = Saves[slot].playerLevel;
        return true;
    }
    
    #endregion

    #region Save Name

    public bool SavePlayerName(string playerName, int slot = 0)
    {
        if (Saves.Count < slot && Saves[slot] == null) return false;
        Saves[slot].playerName = playerName;
        return true;
    }
    
    public bool LoadPlayerName(out string playerName, int slot = 0)
    {
        if (Saves.Count < slot && Saves[slot] == null)
        {
            playerName = "";
            return false;
        }
        playerName = Saves[slot].playerName;
        return true;
    }

    #endregion
    
    public void SaveToFile(int slot = 0)
    {
        File.WriteAllText(dataPath + slot, Encryptor.Encrypt(Saves[slot].ToJson()));
    }

    public bool LoadFromFile(int slot = 0)
    {
        if (!File.Exists(dataPath + slot)) return false;
        Saves[slot].FromJson(Encryptor.Decrypted(File.ReadAllText(dataPath + slot)));
        return true;
    }
    
    [Serializable]
    public class Save
    {
        public int playerLevel;
        public string playerName;
        
        public Save(int playerLevel = 0, string playerName = "")
        {
            this.playerLevel = playerLevel;
            this.playerName = playerName;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void FromJson(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
    
    private class Encryptor
    {
        public static string IV = "1a1a1a1a1a1a1a1a";
        public static string Key = "1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a13";

        public static string Encrypt(string decrypted)
        {
            byte[] textbytes = ASCIIEncoding.ASCII.GetBytes(decrypted);
            AesCryptoServiceProvider endec = new AesCryptoServiceProvider();
            endec.BlockSize = 128;
            endec.KeySize = 256;
            endec.IV = ASCIIEncoding.ASCII.GetBytes(IV);
            endec.Key = ASCIIEncoding.ASCII.GetBytes(Key);
            endec.Padding = PaddingMode.PKCS7;
            endec.Mode = CipherMode.CBC;
            ICryptoTransform icrypt = endec.CreateEncryptor(endec.Key, endec.IV);
            byte[] enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
            icrypt.Dispose();
            return Convert.ToBase64String(enc);
        }

        public static string Decrypted(string encrypted)
        {
            byte[] textbytes = Convert.FromBase64String(encrypted);
            AesCryptoServiceProvider endec = new AesCryptoServiceProvider();
            endec.BlockSize = 128;
            endec.KeySize = 256;
            endec.IV = ASCIIEncoding.ASCII.GetBytes(IV);
            endec.Key = ASCIIEncoding.ASCII.GetBytes(Key);
            endec.Padding = PaddingMode.PKCS7;
            endec.Mode = CipherMode.CBC;
            ICryptoTransform icrypt = endec.CreateDecryptor(endec.Key, endec.IV);
            byte[] enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
            icrypt.Dispose();
            return ASCIIEncoding.ASCII.GetString(enc);
        }
    }

}
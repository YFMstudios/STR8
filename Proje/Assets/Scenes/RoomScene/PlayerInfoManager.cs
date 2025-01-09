using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon; // Photon'un Hashtable kullanımı için
using UnityEngine.SceneManagement;  // Sahne değiştirme için
using TMPro;

public class PlayerInfoManager : MonoBehaviourPunCallbacks
{
    // UI Bileşenleri
    public TMP_InputField playerNameInputField; // Kullanıcı ismi için InputField
    public Button kingdomButton;                // Krallık seçimi için buton
    public GameObject inputPanel;               // Panelin referansı
    public Button enterButton; // Enter butonunun referansı
    private void Start()
{
    // Başlangıçta Krallık butonunu ve InputField'ı görünür yapalım
    kingdomButton.gameObject.SetActive(true);
    playerNameInputField.gameObject.SetActive(true);
    
    // Krallık butonuna tıklama olayı ekle
    kingdomButton.onClick.AddListener(OnKingdomButtonPressed);

    // Enter butonuna tıklama olayı ekle
    enterButton.onClick.AddListener(OnEnterButtonPressed);
}

    public void OnKingdomButtonPressed()
    {
        // 4. ekrana geçiş için sahne değişimi yap
        SceneManager.LoadScene(4);

        // Butonu gizle
        kingdomButton.gameObject.SetActive(false);
    }

  public void OnEnterButtonPressed()
{
    // Kullanıcı ismini al
    string playerName = playerNameInputField.text;

    // Boş isim kontrolü
    if (string.IsNullOrWhiteSpace(playerName))
    {
        Debug.LogWarning("Kullanıcı adı boş bırakılamaz!");
        return;
    }

    // CheckScene scriptinden krallık bilgisini al
    string selectedKingdom = CheckScene.selectedKingdom;

    // Kullanıcı adı ve krallık bilgisiyle birlikte Photon'a gönder
    var customProperties = new Hashtable();
    customProperties["Kingdom"] = selectedKingdom;  // Krallık bilgisini ekle
    customProperties["PlayerName"] = playerName;    // Kullanıcı adını ekle
    customProperties["FoodAmount"] = 0;    // Krallık Gıda 
    customProperties["StoneAmount"] = 0;    // Krallık Tas
    customProperties["GoldAmount"] = 0;    // Krallık Altın
    customProperties["WoodAmount"] = 0;    // Krallık Wood
    customProperties["IronAmount"] = 0;    // Krallık Demir
    customProperties["WarPower"] = 0;    // Krallık SavasGücü
    customProperties["Warisonline"] = false;
    PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

    // Paneli kapat
    inputPanel.SetActive(false);

    // Debug: Photon'a gönderilen bilgileri yazdır
    Debug.Log($"Photon'a gönderilen bilgiler: Kingdom = {selectedKingdom}, PlayerName = {playerName}");

    
}



}
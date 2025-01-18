using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class NextGame : MonoBehaviourPunCallbacks
{
    private string opponentName; // Rakibin ismi

    void Update()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            // Oda bilgileri mevcut
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("war"))
            {
                ExitGames.Client.Photon.Hashtable warInfo = (ExitGames.Client.Photon.Hashtable)PhotonNetwork.CurrentRoom.CustomProperties["war"];
                
                if (warInfo != null)
                {
                    string warOpponentName = warInfo.ContainsKey("opponentName") ? (string)warInfo["opponentName"] : null;
                    Debug.Log("War Bilgileri - Opponent Name: " + warOpponentName);

                    if (warOpponentName != null && warOpponentName == PhotonNetwork.LocalPlayer.NickName)
                    {
                        Debug.Log("Eşleşme bulundu! Kendi isminiz war bilgisinde bulundu.");
                        PhotonNetwork.Disconnect();
                        GoToWarScene();
                    }
                    else
                    {
                        Debug.Log("Eşleşme bulunamadı. OpponentName: " + warOpponentName + ", LocalPlayer: " + PhotonNetwork.LocalPlayer.NickName);
                    }
                }
                else
                {
                    Debug.LogError("War bilgisi geçerli değil.");
                }
            }
            else
            {
                // War bilgisi yoksa yeni ekleyelim
                Debug.Log("'war' bilgisi mevcut değil, yeni ekleniyor...");
                SetWarInfo(""); // Boş bir opponentName ile başlatıyoruz
            }
        }
        else
        {
            Debug.LogError("Oda mevcut değil.");
        }
    }

    // Butona tıklandığında çalışacak olan fonksiyon
    public void goWarScene()
    {
        // Debug: Single-player kontrolü yapılırken
        Debug.Log("Single-player kontrolü başlatılıyor...");

        // Öncelikle Single-player kontrolü yapılır
        if (ScreenTransitions2.ScreenNavigator.previousScreen == "Simple" ||
            ScreenTransitions2.ScreenNavigator.previousScreen == "Mid" ||
            ScreenTransitions2.ScreenNavigator.previousScreen == "Hard")
        {
            // Single-player modunda, direkt 7. sahneye geçiş yap
            Debug.Log("Single-player modunda. 7. ekrana yönlendiriliyor...");
            GoToWarScene();
            return; // Single-player kontrolü sağlandıysa geri kalan işlemler yapılmaz
        }

        // Eğer Single-player değilse, mevcut multiplayer işlemleri devam eder
        Debug.Log("Multiplayer modunda. Rakip kontrolü ve diğer işlemler başlatılıyor...");

        // RegionClickHandler'ın opponentName özelliğinin null olup olmadığını kontrol et
        if (RegionClickHandler.opponentName != null)
        {
            opponentName = RegionClickHandler.opponentName;
            Debug.Log("Opponent Name (Savunan Kişi): " + opponentName);
        }
        else
        {
            Debug.LogError("Rakip adı null! RegionClickHandler'ı kontrol edin.");
            return; // opponentName null ise fonksiyonu erken sonlandırıyoruz
        }

        // Oda bilgilerine war özelliğini opponentName ile ekliyoruz
        Debug.Log("War bilgileri odada güncelleniyor...");
        SetWarInfo(opponentName);

        // War durumu her iki oyuncu için true olarak ayarlanıyor
        Debug.Log("War durumu odada güncelleniyor...");
        PhotonNetwork.Disconnect();
        GoToWarScene();
    }

    // Oda bilgilerine war özelliğini opponentName ile ekliyoruz
    private void SetWarInfo(string opponentName)
    {
        Debug.Log("SetWarInfo fonksiyonu çağrıldı. opponentName: " + opponentName);

        ExitGames.Client.Photon.Hashtable warInfo = new ExitGames.Client.Photon.Hashtable
        {
            { "opponentName", opponentName }
        };

        // Oda bilgilerini güncelliyoruz
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "war", warInfo } // 'war' bilgisini güncelle
        });

        // Debug: War bilgisi güncellendi
        Debug.Log("War bilgileri odada güncellendi. opponentName: " + opponentName);
    }

    // 7. Ekrana gitme fonksiyonu
    private void GoToWarScene()
    {
        try
        {
            // Debug: Sahne yükleniyor
            Debug.Log("7. sahneye yönlendiriliyor...");
            
            // Sahne yükleniyor
            SceneManager.LoadScene(7); // 7. sahne
        }
        catch (System.Exception e)
        {
            // Eğer sahne yüklenirken bir hata olursa
            Debug.LogError("Sahne yüklenirken hata oluştu: " + e.Message);
        }
    }

    // Oda bilgileri güncellendiğinde tetiklenen callback fonksiyonu
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("war"))
        {
            Debug.Log("Oda bilgisi güncellendi. War bilgisi güncellenmiş.");
        }
    }
}

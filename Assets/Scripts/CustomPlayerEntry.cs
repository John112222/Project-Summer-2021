
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using ExitGames.Client.Photon;

public class CustomPlayerEntry : MonoBehaviour
{
    [Header("UI References")]
        public Text PlayerNameText;

        //public Image PlayerColorImage;
        public Button DefenderButton;
        public Button EscaperButton;
        public Image PlayerTeamImage;

        private int ownerId;
        private bool IsDefender;

    // Start is called before the first frame update
    public void OnEnable()
        {
            PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
        }
        private void OnPlayerNumberingChanged()
        {

        }
        public void OnDisable()
        {
            PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
        }

    public void Start()
        {
         if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
            {
                DefenderButton.gameObject.SetActive(false);
                EscaperButton.gameObject.SetActive(false);
                return;
            }
            Hashtable initialProps = new Hashtable() {{GameConfigs.TeamSelection, null}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
                PhotonNetwork.LocalPlayer.SetScore(0);

               DefenderButton.onClick.AddListener(() =>
                {
                    IsDefender = true;
                    SetPlayerTeam(IsDefender);

                    Hashtable props = new Hashtable() {{GameConfigs.TeamSelection, IsDefender}};
                    PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                    if (PhotonNetwork.IsMasterClient)
                    {
                        FindObjectOfType<CustomMainLobby>().LocalPlayerPropertiesUpdated();
                    }
                });
                   EscaperButton.onClick.AddListener(() =>
                {
                    IsDefender = false;
                    SetPlayerTeam(IsDefender);

                    Hashtable props = new Hashtable() {{GameConfigs.TeamSelection, IsDefender}};
                    PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                    if (PhotonNetwork.IsMasterClient)
                    {
                        FindObjectOfType<CustomMainLobby>().LocalPlayerPropertiesUpdated();
                    }
                });

        }


    public void Initialize(int ActorNumber,string nickname)

    {
        ownerId=ActorNumber;
        PlayerNameText.text=nickname;
    }

    public void SetPlayerTeam(bool? MaybeIsDefender)
    
    {
            PlayerTeamImage.enabled=true;
        if(!MaybeIsDefender.HasValue) return;
        
        bool IsDefender = MaybeIsDefender.Value;
        if(IsDefender){
            PlayerTeamImage.color=Color.blue;
        }else{
            PlayerTeamImage.color=Color.red;
        }
    }
}


//using TTSDK;
//using TTSDK.UNBridgeLib.LitJson;
using UnityEngine.UI;
using UnityEngine;



public class SideBarTipUI : InteractivePanelBase
    {
    public int rewardValue;
        public Button rewardBtn;
        public Button enterSideBarBtn;
    private bool isRewardTaken=false;
    // Start is called before the first frame update

    private static SideBarTipUI instance;
    void Awake()
    {
        isRewardTaken = false;
        // Ensure only one instance exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }
    //protected override void Start()
    //    {
    //        base.Start();
    //        CheckEnterFromSideBar();
    //        CheckCanSideBar();

    //    rewardBtn.onClick.AddListener(TakeReward);
    //    }

        // Update is called once per frame
        void Update()
        {
            if (isRewardTaken)
        {
            rewardBtn.gameObject.SetActive(false);
        }
        }

        void TakeReward()
    {


        //TankHealth thrower = GameObject.FindObjectOfType<TankHealth>();
        //if (thrower != null)
        //{
        //    thrower.AddCurrentHealth(rewardValue) ;
        //}
        isRewardTaken = true;



    }
        void CheckCanSideBar()
        {
//            TT.CheckScene(TTSideBar.SceneEnum.SideBar, (b) =>
//            {
//#if UNITY_EDITOR
//            b = true;
//#endif
//            openButton.gameObject.SetActive(b);
//            }, () => { }, (i, s) => { });
        }

        void CheckEnterFromSideBar()
        {
        //    enterSideBarBtn.onClick.AddListener(() =>
        //    {
        //        JsonData jsonData = new JsonData();
        //        jsonData["scene"] = "sidebar"; // 确保以字典形式使用
        //    TT.NavigateToScene(jsonData, null, () =>
        //        {
        //            enterSideBarBtn.gameObject.SetActive(!TikTokGlobalData.IsFromCeBianLan);
        //            rewardBtn.gameObject.SetActive(TikTokGlobalData.IsFromCeBianLan);
        //        }, null);
        //    });
        }
    }


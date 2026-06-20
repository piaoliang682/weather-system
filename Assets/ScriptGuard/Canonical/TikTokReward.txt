using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TTSDK;
using UnityEngine.UI;
public class TikTokReward : MonoBehaviour
{
    public Button restartBtn;
    public Button rewardBtn;
    public int rewardValue=100;
    // Start is called before the first frame update

    private void Awake()
    {
        
    }
    void Start()
    {
        if (rewardBtn != null)
        {
        rewardBtn.onClick.AddListener(OnClickRewardAds);
        }

        if (restartBtn != null)
        {
        restartBtn.onClick.AddListener(OnClickRestartAds);
        }


    }

    void OnClickRewardAds()
    {
        //TTRewardedVideoAd reward = TT.CreateRewardedVideoAd("nso327bywf37kd9078", (b, i) =>
        //{
        //    Debug.Log("close:" + b + " " + i);
        //    if (b)
        //    {
        //        //GetCoin = true;
        //        ThrowProjectile thrower = GameObject.FindObjectOfType<ThrowProjectile>();
        //        if (thrower != null)
        //        {
        //            thrower.ActivateBuff(5f);
        //        }
        //    }
        //}, (count, error) =>
        //{
        //    Debug.Log("error:" + error);
        //});
        //reward.Show();        

    }

    void OnClickRestartAds()
    {
        //TTRewardedVideoAd reward = TT.CreateRewardedVideoAd("nso327bywf37kd9078", (b, i) =>
        //{
        //    Debug.Log("close:" + b + " " + i);
        //    if (b)
        //    {
        //        //GetCoin = true;
        //        //GameManager.instance.getGift();
        //    }
        //}, (count, error) =>
        //{
        //    Debug.Log("error:" + error);
        //});
        //reward.Show();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

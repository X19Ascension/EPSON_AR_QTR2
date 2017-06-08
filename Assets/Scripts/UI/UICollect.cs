using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class UICollect : MonoBehaviour
{

    public Text Data;
    public Text Credits;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Data.text = PersistentData.m_Instance.DataAmount.ToString() + "/" + PersistentData.m_Instance.DataCap.ToString();
        Credits.text = PersistentData.m_Instance.CreditsAmount.ToString() + "/" + PersistentData.m_Instance.CreditsCap.ToString();
    }
}

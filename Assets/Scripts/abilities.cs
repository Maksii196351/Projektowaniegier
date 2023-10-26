using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class abilities : MonoBehaviour
{
    [Header("Dash")]
    public Image imageDash;
    private float cooldown = 5.2f;
    bool isCooldown = false;
   
    void Start()
    {
        imageDash.fillAmount = 0;
    }

    
    void Update()
    {
     dashimage();
    }
    public void dashimage()
    { 
        if(Input.GetKey(KeyCode.LeftShift) && isCooldown == false)
        {
            isCooldown = true;
            imageDash.fillAmount = 1;
        }
        if(isCooldown)
        {
            imageDash.fillAmount -= 1 / cooldown * Time.deltaTime;
            if(imageDash.fillAmount <= 0)
            {
                imageDash.fillAmount = 0;
                isCooldown = false;
            }   
        }
        
    }
}

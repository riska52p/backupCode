using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuSetting : MonoBehaviour
{
    private GameObject gb;
    private GameObject userCamera;

    private GameObject noteParentUI;
    private GameObject keyboardUI;
    private GameObject confirmationUI;
    private Transform uiParent;
    private Transform charaParent;

    private GameObject noteIF;

    private Vector3 deltaPos = Vector3.zero;

    private void Awake()
    {
        gb = this.gameObject;
        userCamera = gb.transform.parent.gameObject;

        noteParentUI = gb.transform.GetChild(1).gameObject;
        keyboardUI = noteParentUI.transform.GetChild(1).gameObject;
        confirmationUI = gb.transform.GetChild(2).gameObject;

        uiParent = gb.transform;
        charaParent = gb.transform.parent.parent.parent.parent;

        noteIF = noteParentUI.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;
    }

    private void Start()
    {
        noteParentUI.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (noteParentUI.activeSelf)
        {
            FollowPlayer(noteParentUI);
        }

        if (confirmationUI.activeSelf)
        {
            FollowPlayer(confirmationUI);
        }
    }

    #region Open Close UI
    public void OpenNote()
    {
        if (!noteParentUI.activeSelf)
        {
            noteParentUI.transform.localRotation = Quaternion.identity;
            noteParentUI.transform.SetParent(charaParent);
            InitializeDeltaPos(noteParentUI.transform.position);
            //ResetRotation(noteParentUI);
            noteParentUI.SetActive(true);
        }
        CloseConfirmation();
    }

    public void CloseNote()
    {
        noteParentUI.SetActive(false);
        noteParentUI.transform.SetParent(uiParent);
    }

    public void ShowHideKeyboard()
    {
        if (keyboardUI.activeSelf)
            keyboardUI.SetActive(false);
        else if (!keyboardUI.activeSelf)
        {
            keyboardUI.SetActive(true);
            keyboardUI.GetComponent<AruKeyboardManager>().inputField = noteIF;
        }
    }

    public void OpenConfirmation()
    {
        //open confirmation
        if (!confirmationUI.activeSelf)
        {
            confirmationUI.transform.SetParent(charaParent);
            InitializeDeltaPos(confirmationUI.transform.position);
            confirmationUI.SetActive(true);
        }
        CloseNote();
    }

    public void CloseConfirmation()
    {
        confirmationUI.SetActive(false);
        confirmationUI.transform.SetParent(uiParent);
    }

    public void ExitButtonClicked()
    {
        CloseConfirmation();
        GameObject script = GameObject.FindGameObjectWithTag("SCRIPT");
        script.GetComponent<NetworkPlayerSpawner>().ExitRoom();
    }

    #endregion

    #region Follow Player

    private void ResetRotation(GameObject uiGB)
    {
        //Quaternion oriRot = uiGB.transform.localRotation;
        //Quaternion identityRot = Quaternion.identity;
        //uiGB.transform.localRotation = new Quaternion(identityRot.x, oriRot.y, identityRot.z, identityRot.w);

        Vector3 oriRot = uiGB.transform.localRotation.eulerAngles;
        uiGB.transform.localRotation = Quaternion.Euler(0, -oriRot.y, 0);
    }

    private void InitializeDeltaPos(Vector3 uiPos)
    {
        if (userCamera != null)
        {
            Vector3 playerPos = userCamera.transform.position;

            deltaPos = new Vector3(uiPos.x - playerPos.x, uiPos.y - playerPos.y, uiPos.z - playerPos.z);
        }
    }

    private void FollowPlayer(GameObject uiGB)
    {
        Vector3 playerPos = userCamera.transform.position;

        Vector3 newPos = new Vector3(playerPos.x + deltaPos.x, playerPos.y, playerPos.z + deltaPos.z);

        uiGB.transform.position = newPos;
    }

    #endregion
}

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
    public static UserInterfaceManager instance { get; private set; }

    [SerializeField]
    private int _defaultMenuIndex;
    private int _previousMenuIndex = -1;
    private int _currentMenuIndex = -1;

    [SerializeField]
    private bool _showMenu = false;

    [SerializeField]
    private GameObject[] m_Menus;

    [SerializeField]
    private GameObject m_PopUpMessage;

    // This is to activate and deactivate action maps when the menu is open or closed. This is to prevent the player from moving while the menu is open.

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (m_Menus != null)
        {
            if (_showMenu)
            {
                m_Menus[_defaultMenuIndex].SetActive(true);
                _currentMenuIndex = _defaultMenuIndex;
            }
        }
    }

    public void GoToMenu(int index)
    {
        if (m_Menus != null)
        {
            _previousMenuIndex = _currentMenuIndex;
            _currentMenuIndex = index;

            if (_previousMenuIndex > -1)
                m_Menus[_previousMenuIndex].SetActive(false);
            m_Menus[index].SetActive(true);
        }
    }

    public void GoBack()
    {
        if (m_Menus != null)
        {
            m_Menus[_currentMenuIndex].SetActive(false);
            m_Menus[_previousMenuIndex].SetActive(true);
            int tempPrevious = _previousMenuIndex;
            _previousMenuIndex = _currentMenuIndex;
            _currentMenuIndex = tempPrevious;
        }
    }

    public void ShowPopUp(string message, UnityAction action)
    {
        if (m_PopUpMessage != null)
        {
            Debug.Log(m_PopUpMessage.transform.GetChild(0));

            m_PopUpMessage.transform.GetChild(0).GetComponent<TMP_Text>().text = message;

            m_PopUpMessage.transform.GetChild(1).GetComponent<Button>().gameObject.SetActive(true);
            m_PopUpMessage.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(action);

            m_PopUpMessage.SetActive(true);
        }
    }

    public void ShowPopUp(string message)
    {
        if (m_PopUpMessage != null)
        {
            Debug.Log(m_PopUpMessage.transform.GetChild(0));

            m_PopUpMessage.transform.GetChild(0).GetComponent<TMP_Text>().text = message;

            m_PopUpMessage.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
            m_PopUpMessage.transform.GetChild(1).GetComponent<Button>().gameObject.SetActive(false);

            m_PopUpMessage.transform.GetChild(2).GetComponentInChildren<TMP_Text>().text = "Ok";

            m_PopUpMessage.SetActive(true);
        }
    }

    public void ClosePopUp()
    {
        if (m_PopUpMessage != null)
        {
            m_PopUpMessage.SetActive(false);
        }
    }

    public void CloseMenu()
    {
        _previousMenuIndex = _currentMenuIndex;
        m_Menus[_currentMenuIndex].SetActive(false);
        _currentMenuIndex = -1;
    }

    public void ForceMenu(int index)
    {
        Debug.Log("Force menu");

        if (_currentMenuIndex > -1)
            m_Menus[_currentMenuIndex].SetActive(false);
        m_Menus[index].SetActive(true);
    }
}

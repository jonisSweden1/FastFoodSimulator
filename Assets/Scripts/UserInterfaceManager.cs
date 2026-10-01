using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
    // Singleton
    public static UserInterfaceManager instance { get; private set; }

    // Set default menu to be visible in the beginning of the scene
    [SerializeField] private int _defaultMenuIndex;

    // Integers for make up orders of the menus
    private int _previousMenuIndex = -1;
    private int _currentMenuIndex = -1;

    // Boolean to show menu as soon as the player starts the scene
    [SerializeField] private bool _showMenu = false;

    // The list of menus that can be added
    [SerializeField] private GameObject[] m_Menus;

    // An individual UI for showing pop up messages for warnings or other information that the player should be warned about
    [SerializeField] private GameObject m_PopUpMessage;

    // This is to activate and deactivate action maps when the menu is open or closed. This is to prevent the player from moving while the menu is open.

    private void Awake()
    {
        // Destroy other instances if they are not from the first instance
        if (instance != null && instance != this)
            Destroy(this);
        // Assign the instance of that
        else
            instance = this;
    }

    private void Start()
    {
        if (m_Menus != null)
        {
            // Show the first menu at the start of the scene
            if (_showMenu)
            {
                m_Menus[_defaultMenuIndex].SetActive(true);
                _currentMenuIndex = _defaultMenuIndex;
            }
        }
    }

    // Go to the menu in the order
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

    // Go back to the previous menu that was in the order
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

    // Show the pop up message with an action whenever the player chooses "yes"
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

    // Show the pop up message without an action
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

    // Close the pop up message
    public void ClosePopUp()
    {
        if (m_PopUpMessage != null)
        {
            m_PopUpMessage.SetActive(false);
        }
    }

    // Close the current menu
    public void CloseMenu()
    {
        _previousMenuIndex = _currentMenuIndex;
        m_Menus[_currentMenuIndex].SetActive(false);
        _currentMenuIndex = -1;
    }

    // Force another menu to pop up and close the other menu
    public void ForceMenu(int index)
    {
        Debug.Log("Force menu");

        if (_currentMenuIndex > -1)
            m_Menus[_currentMenuIndex].SetActive(false);
        m_Menus[index].SetActive(true);
    }
}

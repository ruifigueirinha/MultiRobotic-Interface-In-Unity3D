using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class CustomDropdown : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    // Associar uma toolbar que armazena os dropdowns caso exista
    public ToolbarCustomDropdown MasterToolbar; /* Caso exista uma toolbar parente dos dropdowns, e possivel associa la e assim abrir os restantes dropdowns 
                                    apenas passando o rato caso ja exista um aberto */
                                    
    private RectTransform selfrectTransform, optionsPanelRect, underlineRect;
    [Serializable]
    public class Option : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private string _text;
        private int _index;
        //private Color defaultColor;// = new Color(0.2039216f, 0.2039216f, 0.2039216f, 0.3921569f);
        //private Color highlightColor;// = new Color(0.9137255f, 0.3294118f, 0.1254902f, 1.0f);
        [SerializeField]
        public string text { get { return _text; } set { _text = value; } }
        public int index { get { return _index; } set { _index = value; } }

        public Image img;
        public static Color defaultColor;// = new Color(0.2039216f, 0.2039216f, 0.2039216f, 0.3921569f);
        public static Color highlightColor;// = new Color(0.9137255f, 0.3294118f, 0.1254902f, 1.0f);

        [HideInInspector]
        public CustomDropdown dropdownMainClass;
        public Option(){
            //this.text = text;
            //this.index = index;
            //defaultColor = Color.green;//new Color(0.063f, 0.063f, 0.063f, 0.3921569f);
            //highlightColor = new Color(0.3f, 0.4f, 0.6f, 0.3f); //new Color(0.9137f, 0.3294f, 0.1254f, 1.0f);
            //img = gameObject.transform.GetChild(0).GetComponent<Image>();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            //this.img.color = highlightColor;
            this.img.color = new Color(0.9137f, 0.3294f, 0.1254f, 0.5f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.img.color = Color.clear;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            //CustomDropdown valueEvaluate = new CustomDropdown();
            dropdownMainClass.ValueEvaluate(this.index);
            gameObject.transform.parent.gameObject.SetActive(false);
            //Debug.Log(this.index + " Pressed");
        }

    }

    [SerializeField]
    public List<string> OptionStringList;

    [TextArea]
    [Tooltip("Necessario derivar esta class e implementar o metodo ValueEvaluate")]
    public string Nota = "Necessario derivar esta class e implementar o metodo ValueEvaluate";

    [HideInInspector]
    public List<Image> OptionImagesList;

    private List<Option> OptionList;

    [HideInInspector]
    public GameObject OptionsPanel;
    private GameObject TemplateOption;
    private LayoutElement templateOptionLayoutElement;

    private int maxSize, minSize;

    public int underlinemaxSize = 65;
    private int underlineminSize = 0;
    
    //[HideInInspector]
    //public bool isOpen;
    public virtual void ValueEvaluate(int index){ // tem que ser overriden para proporcionar o codigo que cada um dos items do dropdown corre

    }     

    public void OnPointerClick(PointerEventData eventData)
    {
        //StartCoroutine(ShowOptionsCoroutine());
        ShowOptions();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Start Underline Animation on Enter
        //StopCoroutine(DecreaseUnderlineSize());

        
        if (MasterToolbar.isHovered && MasterToolbar.isDropdownOpen){
            MasterToolbar.AnyDropdownOpen();
            ShowOptions();
        }
        StopAllCoroutines();
        StartCoroutine(IncreaseUnderlineSize());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Start Underline Animation on Exit
        //StopCoroutine(IncreaseUnderlineSize());
        StopAllCoroutines();
        StartCoroutine(DecreaseUnderlineSize());
        StartCoroutine(HideOptionsCoroutine());
        //HideOptions();
    }

    private void ShowOptions()
    {
        OptionsPanel.SetActive(true);
        if(MasterToolbar != null)
            {MasterToolbar.AnyDropdownOpen();}

        //isOpen = true;
    }

    IEnumerator ShowOptionsCoroutine() 
    {
        OptionsPanel.SetActive(true);
        while (optionsPanelRect.sizeDelta.y < OptionList.Count * templateOptionLayoutElement.preferredHeight)
        {
            optionsPanelRect.sizeDelta = new Vector2(optionsPanelRect.sizeDelta.x, optionsPanelRect.sizeDelta.y + 15.0f);
            yield return new WaitForSeconds(1e-9f);
        }  
    }
    IEnumerator HideOptionsCoroutine()
    {
        yield return new WaitForSeconds(.1f);
        HideOptions();
    }
    private void HideOptions()
    {
        OptionsPanel.SetActive(false);
        if (OptionImagesList.Count != 0)
        {
            foreach (var item in OptionImagesList)
            {
                item.color = Color.clear;
            }
        }
    }

    public virtual void PopulateOptions(string[] strings) 
    {
        foreach (var str in strings)
        {
            var instantiatedOption = Instantiate(TemplateOption);
            instantiatedOption.transform.SetParent(OptionsPanel.transform);
            instantiatedOption.GetComponent<TMP_Text>().text = str;
            Option option = instantiatedOption.AddComponent<Option>();
            option.text = str;
            option.index = OptionList.Count;
            option.img = instantiatedOption.transform.GetChild(0).GetComponent<Image>();
            option.dropdownMainClass = this;
            OptionList.Add(option);
            OptionImagesList.Add(option.img);
            instantiatedOption.SetActive(true);
        }
    }
    public virtual void ClearOptions() 
    {
        HideOptions();
        foreach (var item in OptionList) 
        {
            Destroy(item);
        }
        OptionList.Clear();
        foreach (Transform child in OptionsPanel.transform)
        {
            GameObject.Destroy(child.transform.gameObject);
            GameObject.Destroy(child.gameObject);
        }
    }


    #region Underline size enumerators
    IEnumerator IncreaseUnderlineSize(){
        
        while(underlineRect.sizeDelta.x < underlinemaxSize)
        {
            underlineRect.sizeDelta = new Vector2(underlineRect.sizeDelta.x + 5.0f, underlineRect.sizeDelta.y);
            yield return new WaitForSeconds(1e-10f);
        }
    }

    IEnumerator DecreaseUnderlineSize(){
        while (underlineRect.sizeDelta.x > underlineminSize )
        {
            if (underlineRect.sizeDelta.x - 5.0f < underlineminSize)
                {underlineRect.sizeDelta = new Vector2(underlineminSize, underlineRect.sizeDelta.y);}
        else
        {
        underlineRect.sizeDelta = new Vector2(underlineRect.sizeDelta.x - 5.0f, underlineRect.sizeDelta.y);

        }
        yield return new WaitForSeconds(1e-10f);
        }
    }
    #endregion

    #region Dropdown size Enumerators
    IEnumerator IncreaseSize(RectTransform rectTransform)
    {

        while (rectTransform.sizeDelta.x < maxSize)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x + 15.0f, rectTransform.sizeDelta.y + 15.0f);
            yield return new WaitForSeconds(1e-9f);
        }

    }

    IEnumerator DecreaseSize(RectTransform rectTransform)
    {
        while (rectTransform.sizeDelta.x > minSize)
        {
            if (rectTransform.sizeDelta.x - 15.0f < minSize)
                rectTransform.sizeDelta = new Vector2(minSize, rectTransform.sizeDelta.y);
            else
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x - 15.0f, rectTransform.sizeDelta.y - 15.0f);

            }
            yield return new WaitForSeconds(1e-9f);
        }
    }
    #endregion




    // [Serializable]
    // public class DropdownEvent : UnityEngine.Events.UnityEvent<int> { }

    // [SerializeField]
    // private DropdownEvent m_OnValueChanged = new DropdownEvent();
    // public DropdownEvent onValueChanged { get { return m_OnValueChanged; } set { m_OnValueChanged = value; } }
    void Awake()
    {
        //OptionButtonList = new List<Button>();
        OptionList = new List<Option>();
        
        selfrectTransform = gameObject.GetComponent<RectTransform>();
        
        TemplateOption = transform.GetChild(0).gameObject; // template option button
        OptionsPanel = transform.GetChild(1).gameObject; // painel de opçoes
        optionsPanelRect = OptionsPanel.GetComponent<RectTransform>();
        //templateOptionLayoutElement = TemplateOption.GetComponent<LayoutElement>();
        underlineRect = transform.GetChild(2).GetComponent<RectTransform>(); //Rect transform do underline

        if (OptionStringList.Count != 0) {
            foreach (string str in OptionStringList)
            {
                var instantiatedOption = Instantiate(TemplateOption);
                instantiatedOption.transform.SetParent(OptionsPanel.transform);
                instantiatedOption.GetComponent<TMP_Text>().text = str;
                Option option = instantiatedOption.AddComponent<Option>();
                option.text = str;
                option.index = OptionList.Count;
                option.img = instantiatedOption.transform.GetChild(0).GetComponent<Image>();
                option.dropdownMainClass = this;
                OptionList.Add(option);
                OptionImagesList.Add(option.img);
                instantiatedOption.SetActive(true);
            }
        }  
    }
}

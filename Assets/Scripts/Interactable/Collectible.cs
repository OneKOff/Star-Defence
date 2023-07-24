using System;
using TMPro;
using UnityEngine;

public class Collectible : MonoBehaviour, IInteractable
{
    [SerializeField] private WorldUIFollow interactionTipPopupPrefab;
    [SerializeField] private Collider2DInvoker interactionArea;

    private Canvas _worldCanvas;
    private WorldUIFollow _interactionTipPopup;
    
    private void OnEnable()
    {
        _worldCanvas = UIController.Instance.CanvasWorld;
        
        _interactionTipPopup = Instantiate(interactionTipPopupPrefab, _worldCanvas.transform);
        _interactionTipPopup.SetTarget(transform);
        _interactionTipPopup.gameObject.SetActive(false);
        
        interactionArea.TriggerEnter2D += CheckTriggerEnter;
        interactionArea.TriggerStay2D += CheckTriggerStay;
        interactionArea.TriggerExit2D += CheckTriggerExit;
    }
    private void OnDisable()
    {
        interactionArea.TriggerEnter2D -= CheckTriggerEnter;
        interactionArea.TriggerStay2D -= CheckTriggerStay;
        interactionArea.TriggerExit2D -= CheckTriggerExit;
    }

    public virtual void HandleCollecting()
    {
        Debug.Log($"<color=#0FFF0F> {name} collected.");
        gameObject.SetActive(false);
    }
    
    #region Collision Scripts
    public void CheckTriggerEnter(Collider2D other)
    {
        if (!TriggerCheckInit(other, out var plc) || _interactionTipPopup == null) { return; }
        
        _interactionTipPopup.gameObject.SetActive(true);
    }
    public void CheckTriggerStay(Collider2D other)
    {
        if (!TriggerCheckInit(other, out var plc) || !Input.GetKey(KeyCode.E)) { return; }
        
        HandleCollecting();
    }
    public void CheckTriggerExit(Collider2D other)
    {
        if (!TriggerCheckInit(other, out var plc) || _interactionTipPopup == null) { return; }
        
        _interactionTipPopup.gameObject.SetActive(false);
    }
    
    private bool TriggerCheckInit(Collider2D other, out PlayerController plc)
    {
        return other.gameObject.TryGetComponent(out plc);
    }
    #endregion
    
}

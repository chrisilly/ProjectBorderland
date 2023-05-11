using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    PlayerController _playerControll;

    [Header("Stamina")]
    [SerializeField] public float _maxStamina;
    [SerializeField] float _jumpStaminaCost;
    [SerializeField] float _dashingStaminaCost;
    [SerializeField] float _superDashPoint;
    [SerializeField] float _hangWallStaminaCost;
    [SerializeField] float _staminaRegenSpeed;
    [SerializeField] bool _canRegenStamina;
    [SerializeField] public float _stamina;
    [Header("Stamina Bar")]
    [SerializeField] Slider _staminaBar;
    [SerializeField] Slider _superDashStaminaBar;
    [SerializeField] Image _staminaBarFillImage;
    [SerializeField] Image _superDashStaminaBarFillImage;
    private bool _canGainStamina = true;
    private bool _haveEnoughStaminaAction;
    private bool _enableSuperDash;

    void Awake()
    {
        _playerControll = GetComponent<PlayerController>();
    }

    private void Start()
    {
        _stamina = _maxStamina;
        _staminaBar.maxValue = _maxStamina;
        _superDashStaminaBar.maxValue = _superDashPoint;
    }

    void Update()
    {
        StaminaController();
        StaminaBarController();

        ActionStaminaCheck();

        DecreaseStaminaOnJump();
        DecreaseStaminaHoldingWall();
        DecreaseStaminaOnWallMovement();

        EnableSuperDashCehck();
        DecreaseStaminaOnDash();

    }


    /// <summary>
    /// Handle Regeneration of stamina and Min Max of stamina
    /// </summary>
    private void StaminaController()
    {
        // Regeneration of Stamina
        if (_playerControll.IsGrounded())
        {
            if (_stamina <= _maxStamina && _canGainStamina && _canRegenStamina)
                _stamina += _staminaRegenSpeed * Time.deltaTime;
        }

        if (_stamina > _maxStamina)
            _stamina = _maxStamina;
        else if (_stamina <= 0)
            _stamina = 0;
    }
    private void StaminaBarController()
    {
        //Handles Stamina Bar 
        _staminaBar.value = _stamina;
        _superDashStaminaBar.value = _stamina;

        if (_staminaBar.value <= _superDashStaminaBar.minValue)
        {
            _superDashStaminaBarFillImage.enabled = false;
        }
        else
        {
            _superDashStaminaBarFillImage.enabled = true;
        }

        if (_stamina <= _superDashPoint)
        {
            _staminaBarFillImage.enabled = false;
        }
        else
        {
            _staminaBarFillImage.enabled = true;
        }
    }

    private void ActionStaminaCheck()
    {
        if (_stamina > 0)
            _haveEnoughStaminaAction = true;
        else
            _haveEnoughStaminaAction = false;
    }

    private void EnableSuperDashCehck()
    {
        if (_stamina <= _superDashPoint)
        {
            _enableSuperDash = true;
        }
        else
        {
            _enableSuperDash = false;
        }
    }

    #region DECREASE STAMINA
    private void DecreaseStaminaOnJump()
    {
        if (_playerControll.IsDecreasingStaminaOnJump == true)
        {
            _stamina -= _jumpStaminaCost;                   // Stamina only gonna decrease One time  
            _playerControll.IsDecreasingStaminaOnJump = false; // Reset boolean so that it won't decrease stamina multiple times
        }
    }

    private void DecreaseStaminaHoldingWall()
    {
        if (_playerControll.IsDecreasingStaminaHoldWall == true)
        {
            _stamina -= _hangWallStaminaCost * Time.deltaTime;
            _playerControll.IsDecreasingStaminaHoldWall = false;
        }
    }

    // Decreasing Stamina on Wallmovement with a "Magic Number"
    private void DecreaseStaminaOnWallMovement()
    {
        if (_playerControll.IsDecreasingStaminaOnWallMovement == true)
        {
            _stamina -= 2;
            _playerControll.IsDecreasingStaminaOnWallMovement = false;
        }
    }

    private void DecreaseStaminaOnDash()
    {
        if (_playerControll.IsDecreasingStaminaOnDash == true)
        {
            _stamina -= _dashingStaminaCost;
            _playerControll.IsDecreasingStaminaOnDash = false;
        }
    }
    #endregion

    #region PROPERTIES
    /// <summary>
    /// Boolean to check if player have enought stamina to do actions
    /// </summary>
    public bool EnoughStaminaAction
    {
        get { return _haveEnoughStaminaAction; }
    }

    /// <summary>
    /// Player won't be able to gain stamina right after pressed "Jump" or "Dash" while IsGrounded is still true
    /// </summary>
    public bool CanGainStamina
    {
        get { return _canGainStamina; }
        set { _canGainStamina = value; }
    }

    /// <summary>
    /// Cehck if player will perform a Super Dash
    /// </summary>
    public bool CanSuperDash
    {
        get { return _enableSuperDash; }
    }
    #endregion
}

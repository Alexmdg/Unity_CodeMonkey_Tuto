using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using static CuttingCounter;
using static IHasProgress;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateCHangedEventArgs> OnStateChanged;
    public class OnStateCHangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipe;
    private BurningRecipeSO burningRecipe;

    private void Start()
    {
        state = State.Idle;
        OnStateChanged?.Invoke(this, new OnStateCHangedEventArgs{
            state = state
        });
    }

    public void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;

            case State.Frying:
                fryingTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = fryingTimer / fryingRecipe.fryingTimerMax
                });
                if (fryingTimer > fryingRecipe.fryingTimerMax)
                {
                    Debug.Log("Ready !!");
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(fryingRecipe.output, this);
                    burningRecipe = GetBurningRecipe(fryingRecipe.output);
                    if (CanBurn(fryingRecipe.output))
                    {
                        burningRecipe = GetBurningRecipe(fryingRecipe.output);
                        state = State.Fried;
                        burningTimer = 0f;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                            progressNormalized = 0f
                        });
                        break;
                    }
                    state = State.Idle;
                    OnStateChanged?.Invoke(this, new OnStateCHangedEventArgs{
                        state = state
                    });
                }
                break;

            case State.Fried:
                burningTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { 
                   progressNormalized = burningTimer / burningRecipe.burningTimerMax 
                });
                if (burningTimer > burningRecipe.burningTimerMax)
                {
                    Debug.Log("Burnt !!");
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(burningRecipe.output, this);
                    state = State.Burned;
                    OnStateChanged?.Invoke(this, new OnStateCHangedEventArgs{
                        state = State.Idle
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = 0f
                    });
                    break;
                }
                break;
                

            case State.Burned:
                break;
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (CanFry(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipe = GetFryingRecipe(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Frying;
                    OnStateChanged?.Invoke(this, new OnStateCHangedEventArgs
                    {
                        state = state
                    });
                    fryingTimer = 0f;
                }
            }
            else
            {

            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate))
                {
                    if (plate.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateCHangedEventArgs
                        {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateCHangedEventArgs
                {
                    state = state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = 0f
                });
            }
        }
    }

    private KitchenObjectSO GetFryingRecipeOutputFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO recipe = GetFryingRecipe(inputKitchenObjectSO);
        if (recipe != null)
            return recipe.output;
        return null;
    }

    private bool CanFry(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO recipe = GetFryingRecipe(inputKitchenObjectSO);
        return recipe != null;
    }

    private FryingRecipeSO GetFryingRecipe(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO recipe in fryingRecipeArray)
        {
            if (inputKitchenObjectSO == recipe.input)
                return recipe;
        }
        return null;
    }

    private KitchenObjectSO GetBurningRecipeOutputFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        BurningRecipeSO recipe = GetBurningRecipe(inputKitchenObjectSO);
        if (recipe != null)
            return recipe.output;
        return null;
    }

    private bool CanBurn(KitchenObjectSO inputKitchenObjectSO)
    {
        BurningRecipeSO recipe = GetBurningRecipe(inputKitchenObjectSO);
        return recipe != null;
    }

    private BurningRecipeSO GetBurningRecipe(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO recipe in burningRecipeArray)
        {
            if (inputKitchenObjectSO == recipe.input)
                return recipe;
        }
        return null;
    }
}

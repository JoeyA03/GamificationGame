using UnityEngine;

public class StimuliSystem : MonoBehaviour
{
    /*
     * Add to scripts if it will be effected to Stimuli
     * 
     * Player
     * Enemy
     * "Barrels"
     */


    public enum StimuliEffect 
    {
        Oil,
        Burn,
        None
    }

    public StimuliEffect stimuliOnEffect;// hold stimuli
    public float stimuliLingerLength;
    private float stimuliTimer;
    public bool isProvider;             // mark true if it should give stimuli to others;

    public void getEffect(StimuliEffect SE, float L)
    {
        stimuliOnEffect = SE;   //Give stimuli to others
        stimuliLingerLength = L;
    }

    private void Update()
    {
        if (isProvider) return;

        removeStimuli();
    }

    private void removeStimuli()
    {
        
        if (stimuliOnEffect != StimuliEffect.None)
        {
            stimuliTimer += Time.deltaTime;
            if (stimuliTimer >= stimuliLingerLength)
            {
                this.stimuliOnEffect = StimuliEffect.None;
            }
        }
    }
}

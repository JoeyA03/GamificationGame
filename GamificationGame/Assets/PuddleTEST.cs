using UnityEngine;

public class PuddleTEST : MonoBehaviour
{
    public StimuliSystem puddleStimuli;
    [SerializeField] private StimuliSystem testObject;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<StimuliSystem>(out StimuliSystem obj))
        {
            obj.getEffect(puddleStimuli.stimuliOnEffect, puddleStimuli.stimuliLingerLength);
            testObject = obj;


            Debug.Log($"{obj.stimuliOnEffect}, and {obj.stimuliLingerLength}");
        }
    }

    // lol
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<StimuliSystem>(out StimuliSystem obj))
        {
            obj.getEffect(puddleStimuli.stimuliOnEffect, puddleStimuli.stimuliLingerLength);
            testObject = obj;


            Debug.Log($"{obj.stimuliOnEffect}, and {obj.stimuliLingerLength}");
        }

    }

    private void OnTriggerExit(Collider collision)
    {
            testObject = null;
    }

}

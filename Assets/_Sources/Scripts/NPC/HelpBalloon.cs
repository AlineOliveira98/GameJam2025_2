using System.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class HelpBalloon : MonoBehaviour
{
    [SerializeField] private float speedMovement;
    [SerializeField] private int lifeDuration;
    [SerializeField] private Animator anim;
    [SerializeField] private Vector3 followOffset = new Vector3(0, 1.5f, 0);

    private Transform player;

    private async void Start()
    {
        player = GameController.Instance.Player.transform;

        await Task.Delay(lifeDuration * 1000);
        DestroyBalloon();
    }

    private void Update()
    {
        Vector3 targetPos = player.position + followOffset;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speedMovement * Time.deltaTime);        
    } 

    private async void DestroyBalloon()
    {
        if(!gameObject.IsDestroyed())
            anim.SetTrigger("Out");

        var duration = anim.GetCurrentAnimatorClipInfo(0).Length;
        await Task.Delay(duration * 1000);
        if(!gameObject.IsDestroyed()) Destroy(this.gameObject);
    }
}
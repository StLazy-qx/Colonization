using UnityEngine;

public class AnimationPlaybacker : MonoBehaviour
{
    private readonly int _clickOnObjectAnimation = Animator.StringToHash("ClickOnBase");

    [SerializeField] private Animator _animator;

    public void PlayClikAnimation()
    {
        _animator.SetTrigger(_clickOnObjectAnimation);
    }
}

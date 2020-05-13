using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviour
{   

    public static SoundManager soundManager;
    [Header("MusicSound")]
    [SerializeField] Sound[] bgmSounds;   
    [Header("MusicPlay")]
    [SerializeField] AudioSource bgmPlay;  

    [Header("PlayerAction")]  
    [SerializeField] Sound[] playerAction;
    [Header("PlayerState")]
    [SerializeField] AudioSource playerState;
    [Header("ZombieAttack")]
    [SerializeField] Sound[] zombieAttackSounds;
    [Header("ZombieAttackPlay")]
    [SerializeField] AudioSource[] zombieAttackPlay;
    [Header("ZombieAction")]  
    [SerializeField] Sound[] zombieAction;
    [Header("ZombieState")]
    [SerializeField] AudioSource[] zombieState;
    
    public void PlayerAction(string _soundName)
    {
        for (int i = 0; i < playerAction.Length; i++)
        {
            if(playerAction[i].soundName == _soundName)
            {
                if(!playerState.isPlaying)
                {
                    playerState.clip = playerAction[i].clip;
                    playerState.Play();
                    return;
                }
            }
        }
    }
   
    public void ZombieAction(string _soundName)
    {
        for (int i = 0; i < zombieAction.Length; i++)
        {
            if(zombieAction[i].soundName == _soundName)
            {
                for (int x = 0; x < zombieState.Length; x++)
                {
                    if(!zombieState[x].isPlaying)
                    {
                        zombieState[x].clip = zombieAction[i].clip;
                        zombieState[x].Play();
                        return;
                    }
                }
            }
        }
    }
    public void ZombieAttackSound(string _SoundName)
    {
        for (int i = 0; i < zombieAttackSounds.Length; i++)
        {
            if(_SoundName == zombieAttackSounds[i].soundName)
            {
                for (int x = 0; x < zombieAttackPlay.Length; x++)
                {
                    if(!zombieAttackPlay[x].isPlaying)
                    {
                        zombieAttackPlay[x].clip = zombieAttackSounds[i].clip;
                        zombieAttackPlay[x].Play();
                        return;
                    }
                }
            }
        }
    }
     void Start()
     {
        soundManager = this;
     }

     public void SetMusicVolume(float volume)
     {
        bgmPlay.volume = volume;
        bgmPlay.Play();
        bgmPlay.clip = bgmSounds[0].clip;
     }  
   
}

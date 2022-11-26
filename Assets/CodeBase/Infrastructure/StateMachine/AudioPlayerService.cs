﻿using CodeBase.Domain.Enums;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine
{
    public class AudioPlayerService
    {
        private readonly AudioPlayer _audioPlayer;

        public AudioPlayerService(AudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }

        public void PlayVFXAudio(AudioClip audioDataHitAfx, bool oneShot = false)
        {
            if (oneShot)
            {
                _audioPlayer.PlayOneShot(audioDataHitAfx, AudioSourceType.Secondary);
            }
            else
            {
                _audioPlayer.Play(audioDataHitAfx, AudioSourceType.Secondary);
            }
        }
    }
}
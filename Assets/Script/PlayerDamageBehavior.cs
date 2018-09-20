﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

namespace GameJam.Sho
{
    public class PlayerDamageBehavior : MonoBehaviour
    {
        [SerializeField]
        private float knockBackPower = 100.0f;
        [SerializeField]
        private float flushSpan = 1.0f;
        [SerializeField]
        private float flushTime = 1.5f;
        [SerializeField]
        private float cameraShakeTime = 1.0f;
        [SerializeField]
        private float cameraShakeStlength = 1.0f;

        // Use this for initialization
        void Start()
        {
            var player = this.GetComponent<Player>();
            var playerStatus = this.GetComponent<PlayerStatus>();
            var camera = GameObject.Find("Main Camera");

            playerStatus.HPChangedEvent
                .Subscribe(_ =>
                {
                    player.Rigidbody.AddForce((int)player.CurrentDirection * Vector2.left * knockBackPower);
                    StartCoroutine(FlushPlayerSprite(player));
                    camera.transform.transform.DOShakePosition(cameraShakeTime, cameraShakeStlength);
                }).AddTo(this);
        }

        private IEnumerator FlushPlayerSprite(Player player)
        {
            float t = 0.0f;
            while (true)
            {
                yield return new WaitForSeconds(flushSpan);
                player.Renderer.enabled = !player.Renderer.enabled;
                t += Time.deltaTime;
                if (t >= flushTime) break;
            }
            player.Renderer.enabled = true;
        }
    }
}
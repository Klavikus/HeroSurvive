using System;
using System.Reflection;
using GameCore.Source.Controllers.Core;
using GameCore.Source.Controllers.Core.Behaviours;
using GameCore.Source.Domain.EntityComponents;
using GameCore.Source.Presentation.Core.GameLoop;
using UnityEditor;
using UnityEngine;

namespace GameCore.Source.Editor
{
    public class HeroConfigurator : EditorWindow
    {
        private GameObject _heroSource;
        private GameObject _enemySource;
        private LayerMask _layerMask;

        [MenuItem("Tools/HeroConfigurator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(HeroConfigurator));
        }

        private void OnGUI()
        {
            CreateHero();
            CreateEnemy();
        }

        private void CreateHero()
        {
            _heroSource = EditorGUILayout.ObjectField(_heroSource, typeof(GameObject), true) as GameObject;
            _layerMask = EditorGUILayout.LayerField(_layerMask);

            float baseMoveSpeed = 3;
            float transitionMaxDelta = 10;
            float changeColorTime = 0.4f;

            // BaseMoveSpeed = 3
            // WhatIsWall LayerMask(16) = 3
            // TransitionMaxDelta = 10
            // ChangeColorTime = 0.4
            // ColorOnDamage = #FF0C0C

            if (GUILayout.Button(nameof(CreateHero)))
            {
                if (_heroSource == null)
                {
                    Debug.LogWarning("Object not selected!");

                    return;
                }

                InputController inputController = _heroSource.AddComponent<InputController>();
                MoveController moveController = _heroSource.AddComponent<MoveController>();
                SetSerializedField(moveController, "_inputController", inputController);
                SetSerializedField(moveController, "_baseMoveSpeed", baseMoveSpeed);
                SetSerializedField(moveController, "_rigidbody2D", _heroSource.GetComponent<Rigidbody2D>());
                SetSerializedField(moveController, "_spriteRenderer", _heroSource.GetComponent<SpriteRenderer>());
                SetSerializedField(moveController, "_whatIsWall", _layerMask);

                AnimatorSynchronizer animatorSynchronizer = _heroSource.AddComponent<AnimatorSynchronizer>();
                SetSerializedField(animatorSynchronizer, "_inputController", inputController);
                SetSerializedField(animatorSynchronizer, "_animator", _heroSource.GetComponent<Animator>());

                Damageable damageable = _heroSource.AddComponent<Damageable>();
                AbilityHandler abilityHandler = _heroSource.AddComponent<AbilityHandler>();
                PlayerController playerController = _heroSource.AddComponent<PlayerController>();
                SetSerializedField(playerController, "_damageable", damageable);
                SetSerializedField(playerController, "_moveController", moveController);
                SetSerializedField(playerController, "_abilityHandler", abilityHandler);

                HeroView heroView = _heroSource.AddComponent<HeroView>();
                HealthView healthView = _heroSource.AddComponent<HealthView>();
                SetSerializedField(healthView, "Target", _heroSource.transform);
                SetSerializedField(healthView, "TransitionMaxDelta", transitionMaxDelta);

                DamageEffectView damageEffectView = _heroSource.AddComponent<DamageEffectView>();
                SetSerializedField(damageEffectView, "_damageable", damageable);
                SetSerializedField(damageEffectView, "_spriteRenderer", _heroSource.GetComponent<SpriteRenderer>());
                SetSerializedField(damageEffectView, "_changeColorTime", changeColorTime);
            }
        }

        private void CreateEnemy()
        {
            _enemySource = EditorGUILayout.ObjectField(_enemySource, typeof(GameObject), true) as GameObject;

            float baseMoveSpeed = 3;
            float transitionMaxDelta = 10;
            float changeColorTime = 0.4f;

            if (GUILayout.Button(nameof(CreateEnemy)))
            {
                if (_enemySource == null)
                {
                    Debug.LogWarning("Object not selected!");

                    return;
                }

                Damageable damageable = _enemySource.AddComponent<Damageable>();
                EnemyAI enemyAI = _enemySource.AddComponent<EnemyAI>();
                EnemyController enemyController = _enemySource.AddComponent<EnemyController>();
                SetSerializedField(enemyController, "_animator", _enemySource.GetComponent<Animator>());
                SetSerializedField(enemyController, "_damageable", damageable);
                SetSerializedField(enemyController, "_enemyAI", enemyAI);
            }
        }

        private void SetSerializedField<T>(UnityEngine.Object component, string fieldName, T value)
        {
            Type type = component.GetType();
            FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null && field.IsPrivate && field.GetCustomAttributes(typeof(SerializeField), false).Length > 0)
                if (typeof(T) == field.FieldType)
                    field.SetValue(component, value);
                else
                    Debug.LogWarning(
                        $"Type mismatch for field '{fieldName}'. Expected: {field.FieldType}, Provided: {typeof(T)}");
            else
                Debug.LogWarning($"Field '{fieldName}' not found or not marked with [SerializeField] in {type}");
        }
    }
}
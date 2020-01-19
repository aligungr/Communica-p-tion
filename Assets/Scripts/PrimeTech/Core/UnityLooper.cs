using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrimeTech.Core {

    public class UnityLooper : MonoBehaviour {
        public static UnityLooper Instance { get; private set; }

        private Queue<Action> actions;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(this);
                return;
            }

            actions = new Queue<Action>();
        }

        public static void Enqueue(Action action) {
            Instance.enqueue(action);
        }

        private void enqueue(Action action) {
            lock (actions) {
                actions.Enqueue(action);
            }
        }

        private void Update() {
            lock (actions) {
                while (actions.Count > 0) {
                    var action = actions.Dequeue();
                    action?.Invoke();
                }
            }
        }
    }
}
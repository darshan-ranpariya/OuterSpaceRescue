using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uStringComponent_GameObjectState : uStringComponent
{
    [Serializable]
    public class CompCase
    {
        public string keyword;
        public bool matchExact;
        public bool matchCase;
        public bool state;
    }
    public CompCase[] cases = new CompCase[0];
    public bool defaultState = true;

    public override void Handle(ref string s)
    {
        for (int i = 0; i < cases.Length; i++)
        {
            CompCase c = cases[i];

            bool matches = false;
            string so = s;
            string sk = c.keyword;

            if (c.matchCase)
            {
                so = so.ToUpper();
                sk = sk.ToUpper();
            }

            if (c.matchExact) matches = so.Equals(sk);
            else matches = so.Contains(sk);

            if (matches)
            {
                gameObject.SetActive(c.state);
                return;
            }
        }
        gameObject.SetActive(defaultState);
    }
} 
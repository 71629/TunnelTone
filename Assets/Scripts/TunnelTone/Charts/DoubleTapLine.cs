using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TunnelTone.Core;
using TunnelTone.Elements;
using TunnelTone.Events;
using TunnelTone.PlayArea;
using TunnelTone.UI.SongList;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Splines;
using Debug = UnityEngine.Debug;

namespace TunnelTone.Charts
{
    public class DoubleTapLine : MonoBehaviour
    {
        [SerializeField] private Transform NoteRender;
        [SerializeField] private bool isStart;
        [SerializeField] private GameObject [] Taps;
        [SerializeField] private GameObject[] pairObjects;
        [SerializeField] private bool isPair;
        private Color c;
        LineRenderer lineRenderer;
        // Start is called before the first frame update
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.startColor = c;
            lineRenderer.endColor = c;
            c = Color.red;
            isStart = false;
            isPair = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(NoteRender.childCount != 0 && isStart == false)
            {
                Taps = new GameObject[countTap()];
                insertTap();
                pairTaps();
                isStart = true;
            }
            if (isStart)
            {
                pairTapsLineDraw();
            }
        }
        private void pairTapsLineDraw()
        {
            lineRenderer.positionCount = pairObjects.Length;

            for (int i = 0; i < pairObjects.Length; i++)
            {
                if (pairObjects[i] != null && pairObjects[i + 1] != null)
                {
                    lineRenderer.SetPosition(i, pairObjects[i].transform.position);
                }
                
            }
        }
        private void pairTaps() // pair Taps array that have the same time
        {
            int count = 0;
            pairObjects = new GameObject[Taps.Length];
            for (int i=0; i<Taps.Length; i++)
            {
                for(int j=0; j<Taps.Length; j++)
                {
                    if ((Taps[i].GetComponent<Tap>().time == Taps[j].GetComponent<Tap>().time) && (Taps[i] != Taps[j]))
                    {
                        isPair = true;
                        for (int k = 0; k < pairObjects.Length; k++)
                        {
                            if (Taps[i] == pairObjects[k] || Taps[j] == pairObjects[k])
                            {
                                isPair = false;
                            }
                        }
                        if (isPair)
                        {
                            
                            pairObjects[count] = Taps[i];
                            //pairObjects[count].GetComponent<Image>().color = c; //Change the pair taps in to red
                            count++;
                            pairObjects[count] = Taps[j];
                            //pairObjects[count].GetComponent<Image>().color = c; //Change the pair taps in to red
                            count++;
                        }
                    }
                }
            }
            Taps = new GameObject[0]; //Release array memory
            //Taps = new GameObject[count / 2]; //This is the fuckup shit no use anymore
            //count = 0;
            //for(int i=0; i < Taps.Length; i++)
            //{
                //Taps[i] = pairObjects[count];
                //count++;
                //i++;
                //Taps[i] = pairObjects[count];
                //count++;
                //count++;
                //count++;
            //}
        }
        private int countTap() //Count the tap number
        {
            int tapNumber = 0;
            if (NoteRender.childCount != 0)
            {
                for (int i = 0; i < NoteRender.childCount; i++)
                {
                    for (int j = 0; j < NoteRender.GetChild(i).childCount; j++)
                    {
                        if (NoteRender.GetChild(i).GetChild(j).name.Equals("Tap(Clone)"))
                        {
                            tapNumber++;
                        }
                    }
                }
            }
            return tapNumber;
        }
        private void insertTap() //Find Tap(Clone) in NoteContainer and save into Tap array
        {
            int tapIndex = 0;

            for (int i = 0; i < NoteRender.childCount; i++)
            {
                for (int j = 0; j < NoteRender.GetChild(i).childCount; j++)
                {
                    if (NoteRender.GetChild(i).GetChild(j).name.Equals("Tap(Clone)"))
                    {
                        Taps[tapIndex] = NoteRender.GetChild(i).GetChild(j).gameObject;
                        tapIndex++;
                    }
                }
            }
        }
    }
}

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
        private Vector3[] position;
        private Color c;
        LineRenderer lineRenderer;
        // Start is called before the first frame update
        void Start()
        {
            position = new Vector3[2];
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
                insertTap(); //Collect all taps inside the Trail
                pairTaps(); //Match the taps that have the same time
                pairTapsLineDraw();//Draw map line
                isStart = true;
            }
            if (isStart)
            {
                pairTapsLineUpdate(); //Update line position per frame when start
            }
            if(NoteRender.childCount == 0)
            {
                isStart = false;
            }
        }
        private void pairTapsLineUpdate()
        {
            for (int i = 0; i < pairObjects.Length; i++)
            {
                if (pairObjects[i] != null && pairObjects[i + 1] != null)
                {
                    position[0] = pairObjects[i].transform.position;
                    lineRenderer = pairObjects[i].GetComponent<LineRenderer>();
                    i++;
                    position[1] = pairObjects[i].transform.position;
                    lineRenderer.SetPositions(position);
                }
            }
        }
        private void pairTapsLineDraw()
        {
            for (int i = 0; i < pairObjects.Length; i++)
            {
                if (pairObjects[i] != null && pairObjects[i + 1] != null)
                {
                    pairObjects[i].AddComponent<LineRenderer>();
                    lineRenderer = pairObjects[i].GetComponent<LineRenderer>();
                    position[0] = pairObjects[i].transform.position;
                    i++;
                    position[1] = pairObjects[i].transform.position;
                    lineRenderer.SetPositions(position);
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
                            pairObjects[count].GetComponent<Image>().color = c; //Change the pair taps in to red and VERY USEFUL TO DEBUG A MAP
                            count++;
                            pairObjects[count] = Taps[j];
                            pairObjects[count].GetComponent<Image>().color = c; //Change the pair taps in to red and VERY USEFUL TO DEBUG A MAP
                            count++;
                        }
                    }
                }
            }
            Taps = new GameObject[0]; //Release array memory
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
                        if (NoteRender.GetChild(i).GetChild(j).GetComponent<Tap>() != null)
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
                    if (NoteRender.GetChild(i).GetChild(j).GetComponent<Tap>() != null)
                    {
                        Taps[tapIndex] = NoteRender.GetChild(i).GetChild(j).gameObject;
                        tapIndex++;
                    }
                }
            }
        }
    }
}

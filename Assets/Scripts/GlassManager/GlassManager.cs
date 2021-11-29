using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GlassManager
{
    public class GlassManager : MonoBehaviour
    {
        // Start is called before the first frame update
        private List<Breakable> breakableWindows;
        private List<Breakable> leftBreakableWindows;
        private List<Breakable> rightBreakableWindows;

        private void Awake()
        {
            breakableWindows = FindObjectsOfType<Breakable>().ToList();
        }

        private void Start()
        {
            MirrorSateInitializer(breakableWindows);

            Debug.Log("windows" + breakableWindows.Count);
        }

        // Update is called once per frame
        private void Update()
        {
        }

        private void MirrorSateInitializer(List<Breakable> windows)
        {
            leftBreakableWindows = breakableWindows.Where(w =>  w.transform.parent.name.Contains("Left")).OrderByDescending(w => w.name)
                .ToList();
            rightBreakableWindows = breakableWindows.Where(w =>w.transform.parent.name.Contains("Right")).OrderByDescending(w => w.name)
                .ToList();
            Debug.Log("windowsLeft" + leftBreakableWindows.Count);
            Debug.Log("windowsRight" + rightBreakableWindows.Count);


            var i = 0;


            foreach (var t in leftBreakableWindows)
            {
                if (i % 2 == 0)
                {
                    t.BreakOnCollision = false;
                }

                i++;
                Debug.Log(" left window name " + t.name);
            }

            i = 0;
            foreach (var t in rightBreakableWindows)
            {
                if (i % 2 != 0)
                {
                    t.BreakOnCollision = false;
                }

                i++;
                Debug.Log(" right window name " + t.name);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using KMHelper;
using System.Text.RegularExpressions;
using System;

public class FursonaScript : MonoBehaviour
{
    public MeshRenderer CroppedRenderer, HeadRenderer, EyesRenderer, FleshRenderer, PrimaryRenderer, SecondaryRenderer, TertiaryRenderer;

    public PhysicalSlider A1, A2, A3;
    public PhysicalSlider B1, B2, B3;
    public PhysicalSlider C1, C2, C3;
    public PhysicalSlider D1, D2, D3;
    public PhysicalSlider E1, E2, E3;
    public PhysicalSlider F1, F2, F3;

    public Material[] CroppedMats, HeadMats, EyesMats, FleshMats, PrimaryMats, SecondaryMats, TertiaryMats;
    private static readonly string[] NAMES = new string[] { "Bat", "Canine", "Dragon", "Goat", "Lion", "Manokit", "Protogen", "Sergal", "Skulldog", "Tiger" };

    private int speciesId = 0;

    private static readonly string[] COLORNAMES = new string[] { "Red", "Green", "Blue", "Cyan", "Magenta", "Yellow" };
    private static readonly Colors[] COLORS = new Colors[] { Colors.Red, Colors.Green, Colors.Blue, Colors.Cyan, Colors.Magenta, Colors.Yellow };

    private int _id;
    private static int idCounter = 1;

    private int TargetEyeColor = 0;
    private bool _Solved = false;

    // Use this for initialization
    void Start()
    {
        _id = idCounter++;
        speciesId = UnityEngine.Random.Range(0, CroppedMats.Length - 1);
        Debug.LogFormat("[Fursona #{0}] Selected species: {1}", _id, NAMES[speciesId]);
        CroppedRenderer.material = CroppedMats[speciesId];
        HeadRenderer.material = HeadMats[speciesId];
        EyesRenderer.material = EyesMats[speciesId];
        FleshRenderer.material = FleshMats[speciesId];
        PrimaryRenderer.material = PrimaryMats[speciesId];
        SecondaryRenderer.material = SecondaryMats[speciesId];
        TertiaryRenderer.material = TertiaryMats[speciesId];
        UpdateColors();
        PhysicalSlider[] Sliders = new PhysicalSlider[] { A1, A2, A3, B1, B2, B3, C1, C2, C3, D1, D2, D3, E1, E2, E3, F1, F2, F3 };
        foreach(PhysicalSlider Slider in Sliders)
            Slider.OnUpdate += UpdateColors;

        KMBombInfo info = GetComponent<KMBombInfo>();
        int ui = KMBombInfoExtensions.GetOffIndicators(info).Count();
        int li = KMBombInfoExtensions.GetOnIndicators(info).Count();
        int b = KMBombInfoExtensions.GetBatteryCount(info);
        int bh = KMBombInfoExtensions.GetBatteryHolderCount(info);
        int p = KMBombInfoExtensions.GetPortCount(info);
        int ep = KMBombInfoExtensions.GetPortPlates(info).Count(sa => sa.Length == 0);
        int s3 = int.Parse(KMBombInfoExtensions.GetSerialNumber(info)[2].ToString());
        int s6 = int.Parse(KMBombInfoExtensions.GetSerialNumber(info)[5].ToString());
        int pp = KMBombInfoExtensions.GetPortPlates(info).Count();
        int sl = KMBombInfoExtensions.GetSerialNumberLetters(info).Count();
        int sn = KMBombInfoExtensions.GetSerialNumberNumbers(info).Count();
        int m = info.GetModuleIDs().Count;

        int R = -1, G = -1, B = -1, C = -1, M = -1, Y = -1;
        switch(speciesId)
        {
            case 0:
                R = li;
                G = ui;
                B = b;
                C = bh;
                M = p;
                Y = ep;
                break;
            case 1:
                R = s3;
                G = s6;
                B = pp;
                C = sl;
                M = sn;
                Y = m;
                break;
            case 2:
                R = ep;
                G = m;
                B = p;
                C = sn;
                M = bh;
                Y = sl;
                break;
            case 3:
                R = sl;
                G = s3;
                B = bh;
                C = li;
                M = sn;
                Y = s6;
                break;
            case 4:
                R = p;
                G = ui;
                B = m;
                C = pp;
                M = ep;
                Y = b;
                break;
            case 5:
                R = s6;
                G = b;
                B = sn;
                C = ep;
                M = li;
                Y = pp;
                break;
            case 6:
                R = bh;
                G = m;
                B = s3;
                C = ui;
                M = sl;
                Y = p;
                break;
            case 7:
                R = pp;
                G = p;
                B = li;
                C = sl;
                M = ep;
                Y = ui;
                break;
            case 8:
                R = sn;
                G = s3;
                B = b;
                C = m;
                M = s6;
                Y = bh;
                break;
            case 9:
                R = ui;
                G = bh;
                B = ep;
                C = s6;
                M = sl;
                Y = m;
                break;
            case 10:
                R = li;
                G = b;
                B = p;
                C = s3;
                M = pp;
                Y = sn;
                break;
            default:
                break;
        }

        int maxId = 0;
        int[] vals = new int[] { R, G, B, C, M, Y };
        for(int i = 1; i < 6; i++)
            if(vals[i] >= vals[maxId])
                maxId = i;

        TargetEyeColor = maxId;
        Debug.LogFormat("[Fursona #{0}] Eye color: {1}; Head color: {2}", _id, COLORNAMES[maxId], COLORNAMES[(maxId + 3) % 6]);
    }

    private void UpdateColors()
    {
        HeadRenderer.material.color = new Color(A1.Value, A2.Value, A3.Value);
        EyesRenderer.material.color = new Color(B1.Value, B2.Value, B3.Value);
        PrimaryRenderer.material.color = new Color(C1.Value, C2.Value, C3.Value);
        SecondaryRenderer.material.color = new Color(D1.Value, D2.Value, D3.Value);
        TertiaryRenderer.material.color = new Color(E1.Value, E2.Value, E3.Value);
        FleshRenderer.material.color = new Color(F1.Value, F2.Value, F3.Value);
        Debug.LogFormat("<Fursona #{0}> Colors set to: {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14} {15} {16}", _id, A1.Value, A2.Value, A3.Value, B1.Value, B2.Value, B3.Value, C1.Value, C2.Value, C3.Value, D1.Value, D2.Value, D3.Value, E1.Value, E2.Value, E3.Value, F1.Value, F2.Value, F3.Value);
        CheckColors();
    }

    private void CheckColors()
    {
        if(new Colors[] { CheckColor(A1.Value, A2.Value, A3.Value), CheckColor(B1.Value, B2.Value, B3.Value), CheckColor(C1.Value, C2.Value, C3.Value), CheckColor(D1.Value, D2.Value, D3.Value), CheckColor(E1.Value, E2.Value, E3.Value), CheckColor(F1.Value, F2.Value, F3.Value) }.All(c => c == Colors.Blue))
        {
            CroppedRenderer.material = CroppedMats[speciesId];
            HeadRenderer.material = HeadMats[speciesId];
            EyesRenderer.material = EyesMats[speciesId];
            FleshRenderer.material = FleshMats[speciesId];
            PrimaryRenderer.material = PrimaryMats[speciesId];
            SecondaryRenderer.material = SecondaryMats[speciesId];
            TertiaryRenderer.material = TertiaryMats[speciesId];
            HeadRenderer.material.color = new Color(A1.Value, A2.Value, A3.Value);
            EyesRenderer.material.color = new Color(B1.Value, B2.Value, B3.Value);
            PrimaryRenderer.material.color = new Color(C1.Value, C2.Value, C3.Value);
            SecondaryRenderer.material.color = new Color(D1.Value, D2.Value, D3.Value);
            TertiaryRenderer.material.color = new Color(E1.Value, E2.Value, E3.Value);
            FleshRenderer.material.color = new Color(F1.Value, F2.Value, F3.Value);
        }
        if(new Colors[] { CheckColor(A1.Value, A2.Value, A3.Value), CheckColor(B1.Value, B2.Value, B3.Value), CheckColor(C1.Value, C2.Value, C3.Value), CheckColor(D1.Value, D2.Value, D3.Value), CheckColor(E1.Value, E2.Value, E3.Value), CheckColor(F1.Value, F2.Value, F3.Value) }.All(c => c == Colors.Yellow))
        {
            CroppedRenderer.material = CroppedMats.Last();
            HeadRenderer.material = HeadMats.Last();
            EyesRenderer.material = EyesMats.Last();
            FleshRenderer.material = FleshMats.Last();
            PrimaryRenderer.material = PrimaryMats.Last();
            SecondaryRenderer.material = SecondaryMats.Last();
            TertiaryRenderer.material = TertiaryMats.Last();
            HeadRenderer.material.color = new Color(A1.Value, A2.Value, A3.Value);
            EyesRenderer.material.color = new Color(B1.Value, B2.Value, B3.Value);
            PrimaryRenderer.material.color = new Color(C1.Value, C2.Value, C3.Value);
            SecondaryRenderer.material.color = new Color(D1.Value, D2.Value, D3.Value);
            TertiaryRenderer.material.color = new Color(E1.Value, E2.Value, E3.Value);
            FleshRenderer.material.color = new Color(F1.Value, F2.Value, F3.Value);
        }
        if(!_Solved && CheckColor(B1.Value, B2.Value, B3.Value) == COLORS[TargetEyeColor] && CheckColor(A1.Value, A2.Value, A3.Value) == COLORS[(TargetEyeColor + 3) % 6])
        {
            if(new Colors[] { CheckColor(A1.Value, A2.Value, A3.Value), CheckColor(B1.Value, B2.Value, B3.Value), CheckColor(C1.Value, C2.Value, C3.Value), CheckColor(D1.Value, D2.Value, D3.Value), CheckColor(E1.Value, E2.Value, E3.Value), CheckColor(F1.Value, F2.Value, F3.Value) }.Where(c => c != Colors.Err).Distinct().Count() >= 6)
                Solve();
        }
    }

    private void Solve()
    {
        if(_Solved) return;
        Debug.LogFormat("[Fursona #{0}] Beautiful 'sona! Solved.", _id);
        GetComponent<KMBombModule>().HandlePass();
    }

    private Colors CheckColor(float r, float g, float b)
    {
        if((r > 0.3f && r < 0.7f) || (g > 0.3f && g < 0.7f) || (b > 0.3f && b < 0.7f))
            return Colors.Err;
        if(r <= 0.3f)
        {
            if(g <= 0.3f)
            {
                if(b <= 0.3f) return Colors.Err;
                else return Colors.Blue;
            }
            else
            {
                if(b <= 0.3f) return Colors.Green;
                else return Colors.Cyan;
            }
        }
        else
        {
            if(g <= 0.3f)
            {
                if(b <= 0.3f) return Colors.Red;
                else return Colors.Magenta;
            }
            else
            {
                if(b <= 0.3f) return Colors.Yellow;
                else return Colors.Err;
            }
        }
    }

    private enum Colors
    {
        Err,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow
    }

#pragma warning disable 414
    private string TwitchHelpMessage = "Use '!{0} #012ABC #789 #FFFF00 #0F0 #234 #FDC987' to set every slider to those hex color values. Use '!{0} 3 #FFF' to set the third set of sliders to that color value.";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.Trim().ToLowerInvariant();

        Regex r = new Regex("((?:#[0-9A-Fa-f]{3} |#[0-9A-Fa-f]{6} ){5}(?:#[0-9A-Fa-f]{3}|#[0-9A-Fa-f]{6}))");
        Regex rs = new Regex("([1-6]) ?(#[0-9A-Fa-f]{3}|#[0-9A-Fa-f]{6})");

        Match rm = r.Match(command);
        Match rsm = rs.Match(command);
        if(rsm.Success)
        {
            int ix;
            if(!int.TryParse(rsm.Groups[1].Value, out ix))
                yield break;
            if(ix < 1 || ix > 6)
                yield break;
            foreach(object e in SetColor(ix, rsm.Groups[2].Value))
            {
                if(e is ColorError)
                    yield break;
                yield return e;
            }
            yield return null;
            yield break;
        }
        if(rm.Success)
        {
            string[] vals = rm.Groups[1].Value.Split(' ').Where(s => s.Length > 2).ToArray();
            for(int ix = 1; ix < 7; ix++)
            {
                foreach(object e in SetColor(ix, vals[ix - 1]))
                {
                    if(e is ColorError)
                        yield break;
                    yield return e;
                }
            }
            yield return null;
            yield break;
        }
    }

    private IEnumerable SetColor(int ix, string value)
    {
        PhysicalSlider[] sliders =
            ix == 1 ? new[] { A1, A2, A3 } :
            ix == 2 ? new[] { B1, B2, B3 } :
            ix == 3 ? new[] { C1, C2, C3 } :
            ix == 4 ? new[] { D1, D2, D3 } :
            ix == 5 ? new[] { E1, E2, E3 } :
            ix == 6 ? new[] { F1, F2, F3 } :
            null;
        if(sliders == null)
            yield return new ColorError();

        Color c;
        if(!ColorUtility.TryParseHtmlString(value, out c))
            yield return new ColorError();

        sliders[0].Value = c.r;
        sliders[0].UpdateFollower();
        yield return new WaitForSeconds(0.1f);
        sliders[1].Value = c.g;
        sliders[1].UpdateFollower();
        yield return new WaitForSeconds(0.1f);
        sliders[2].Value = c.b;
        sliders[2].UpdateFollower();
        yield return new WaitForSeconds(0.1f);
    }

    private class ColorError { }

    IEnumerator TwitchHandleForcedSolve()
    {
        Colors c0 = COLORS[(TargetEyeColor + 3) % 6];
        Colors c1 = COLORS[TargetEyeColor];

        List<Colors> r = new[] { Colors.Red, Colors.Green, Colors.Blue, Colors.Cyan, Colors.Magenta, Colors.Yellow }.Where(c => c != c0 && c != c1).ToList().Shuffle();
        r.Insert(0, c1);
        r.Insert(0, c0);

        foreach(Colors c in r)
            foreach(object e in SetColor(r.IndexOf(c) + 1, ColorsToHtml(c)))
            {
                if(e is ColorError)
                    throw new Exception("SetColor() threw an error. That's all we know.");
                yield return e;
            }
    }

    private string ColorsToHtml(Colors c)
    {
        return c == Colors.Blue ? "#00F" :
            c == Colors.Red ? "#F00" :
            c == Colors.Green ? "#0F0" :
            c == Colors.Cyan ? "#0FF" :
            c == Colors.Magenta ? "#F0F" :
            c == Colors.Yellow ? "#FF0" :
            "";
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class JoyconManager : MonoBehaviour
{
  // Settings accessible via Unity
  public bool EnableIMU = true;
  public bool EnableLocalize = true;

  // Different operating systems either do or don't like the trailing zero
  private const ushort vendor_id = 0x57e;
  private const ushort vendor_id_ = 0x057e;
  private const ushort product_l = 0x2006;
  private const ushort product_r = 0x2007;

  public List<Joycon> Joycons; // Array of all connected Joy-Cons
  static JoyconManager instance;

  public static JoyconManager Instance
  {
    get { return instance; }
  }

  void Awake()
  {
    if (instance != null) Destroy(gameObject);
    instance = this;

    Joycons = new List<Joycon>();
    var init = HIDapi.hid_init();
    if (init != 0)
    {
      Debug.Log($"Failed to initialise HIDAPI! hid_init returned {init}");
    }
    else
    {
      Debug.Log("HIDAPI initialised successfully");
    };

    IntPtr ptr = HIDapi.hid_enumerate(0x0, 0x0);
    IntPtr top_ptr = ptr;

    if (ptr == IntPtr.Zero)
    {
      ptr = HIDapi.hid_enumerate(0x0, 0x0);
      if (ptr == IntPtr.Zero)
      {
        HIDapi.hid_free_enumeration(ptr);
        Debug.Log("No Joy-Cons found!");
        var leftJoycon = HIDapi.hid_open(vendor_id, product_l, null);
        if (leftJoycon != IntPtr.Zero) {
          AddJoycon(leftJoycon, true);
          Debug.Log($"Left Joy-Con connected at index {Joycons.Count}");
        }
        var rightJoycon = HIDapi.hid_open(vendor_id, product_r, null);
        if (rightJoycon != IntPtr.Zero) {
          AddJoycon(rightJoycon, true);
          Debug.Log($"Right Joy-Con connected at index {Joycons.Count}");
        }
        if (leftJoycon == IntPtr.Zero && rightJoycon == IntPtr.Zero) {
          Debug.Log("Direct attempts to access Joycons failed!");
        }
      }
    }
    hid_device_info enumerate;
    while (ptr != IntPtr.Zero)
    {
      enumerate = (hid_device_info)Marshal.PtrToStructure(ptr, typeof(hid_device_info));

      if (enumerate.product_id == product_l)
      {
        AddJoycon(enumerate.path, true);
        Debug.Log($"Left Joy-Con connected at index {Joycons.Count}");
      }
      else if (enumerate.product_id == product_r)
      {
        AddJoycon(enumerate.path, false);
        Debug.Log($"Right Joy-Con connected at index {Joycons.Count}");
      }
      else
      {
        Debug.Log($"Non Joy-Con input device {enumerate.vendor_id:x}:{enumerate.product_id:x} skipped.");
      }
      ptr = enumerate.next;
    }
    HIDapi.hid_free_enumeration(top_ptr);
  }

  void AddJoycon(string path, bool isLeft) {
    IntPtr handle = HIDapi.hid_open_path(path);
    HIDapi.hid_set_nonblocking(handle, 1);
    Joycons.Add(new Joycon(handle, EnableIMU, EnableLocalize & EnableIMU, 0.05f, isLeft));
  }

  void AddJoycon(IntPtr handle, bool isLeft) {
    HIDapi.hid_set_nonblocking(handle, 1);
    Joycons.Add(new Joycon(handle, EnableIMU, EnableLocalize & EnableIMU, 0.05f, isLeft));
  }

  void Start()
  {
    byte LEDs = 0x1;
    Joycons.ForEach((joyCon) => {
      joyCon.Attach(leds_: LEDs);
      joyCon.Begin();
      LEDs = (byte)(LEDs << 1);
    });
  }

  void Update()
  {
    Joycons.ForEach(joyCon => joyCon.Update());
  }

  void OnApplicationQuit()
  {
    Joycons.ForEach(joyCon => joyCon.Detach());
  }
}

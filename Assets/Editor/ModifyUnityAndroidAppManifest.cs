using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor.Android;
using UnityEngine;

public class ModifyUnityAndroidAppManifest : IPostGenerateGradleAndroidProject {
    public int callbackOrder => 1;

    public void OnPostGenerateGradleAndroidProject(string basePath) {
        var androidManifest = new AndroidManifest(GetManifestPath(basePath));
        androidManifest.SetMicrophonePermission();
        androidManifest.Save();
    }

    private string GetManifestPath(string basePath) {
        var pathBuilder = new StringBuilder(basePath);
        pathBuilder.Append(Path.DirectorySeparatorChar).Append("src");
        pathBuilder.Append(Path.DirectorySeparatorChar).Append("main");
        pathBuilder.Append(Path.DirectorySeparatorChar).Append("AndroidManifest.xml");
        return pathBuilder.ToString();
    }
}

internal class AndroidXmlDocument : XmlDocument {
    private string m_Path;
    protected XmlNamespaceManager nsMgr;
    public readonly string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";
    public AndroidXmlDocument(string path) {
        m_Path = path;
        using (var reader = new XmlTextReader(m_Path)) {
            reader.Read();
            Load(reader);
        }
        nsMgr = new XmlNamespaceManager(NameTable);
        nsMgr.AddNamespace("android", AndroidXmlNamespace);
    }

    public string Save() {
        return SaveAs(m_Path);
    }

    public string SaveAs(string path) {
        using (var writer = new XmlTextWriter(path, new UTF8Encoding(false))) {
            writer.Formatting = Formatting.Indented;
            Save(writer);
        }
        return path;
    }
}

internal class AndroidManifest : AndroidXmlDocument {
    private readonly XmlElement ApplicationElement;

    public AndroidManifest(string path) : base(path) {
        ApplicationElement = SelectSingleNode("/manifest/application") as XmlElement;
    }

    private XmlAttribute CreateAndroidAttribute(string key, string value) {
        XmlAttribute attr = CreateAttribute("android", key, AndroidXmlNamespace);
        attr.Value = value;
        return attr;
    }

    internal XmlNode GetActivityWithLaunchIntent() {
        return SelectSingleNode("/manifest/application/activity[intent-filter/action/@android:name='android.intent.action.MAIN' and " +
                "intent-filter/category/@android:name='android.intent.category.LAUNCHER']", nsMgr);
    }

    internal void SetApplicationTheme(string appTheme) {
        ApplicationElement.Attributes.Append(CreateAndroidAttribute("theme", appTheme));
    }

    internal void SetStartingActivityName(string activityName) {
        GetActivityWithLaunchIntent().Attributes.Append(CreateAndroidAttribute("name", activityName));
    }

    internal void SetHardwareAcceleration() {
        GetActivityWithLaunchIntent().Attributes.Append(CreateAndroidAttribute("hardwareAccelerated", "true"));
    }

    internal void SetMicrophonePermission() {
        var manifest = SelectSingleNode("/manifest");
        XmlElement child = CreateElement("uses-permission");
        manifest.AppendChild(child);
        XmlAttribute newAttribute = CreateAndroidAttribute("name", "android.permission.RECORD_AUDIO");
        child.Attributes.Append(newAttribute);
    }
}
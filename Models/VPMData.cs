using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sonic853.Models
{
    /// <summary>
    /// VPM 数据
    /// </summary>
    public class VPMData
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name = string.Empty;
        /// <summary>
        /// 作者
        /// </summary>
        public string author = string.Empty;
        /// <summary>
        /// ID
        /// </summary>
        public string id = string.Empty;
        /// <summary>
        /// 链接
        /// </summary>
        public string url = string.Empty;
        /// <summary>
        /// 包列表
        /// </summary>
        /// <typeparam name="string">包名称</typeparam>
        /// <typeparam name="VPMDataPackage">包</typeparam>
        /// <returns>包</returns>
        public Dictionary<string, VPMDataPackage> packages = new();
    }
    /// <summary>
    /// 包
    /// </summary>
    public class VPMDataPackage
    {
        /// <summary>
        /// 版本列表
        /// </summary>
        /// <typeparam name="string">版本号</typeparam>
        /// <typeparam name="PackageVersion">该版本的包</typeparam>
        /// <returns>该版本的包</returns>
        public Dictionary<string, PackageVersion> versions = new();
    }
    /// <summary>
    /// 包扩展
    /// </summary>
    public static class VPMDataPackageExtensions
    {
        /// <summary>
        /// 获取所有包版本
        /// </summary>
        /// <param name="package">包</param>
        /// <returns>包的所有版本</returns>
        public static List<KeyValuePair<string, PackageVersion>> GetVersions(this VPMDataPackage package)
        {
            List<KeyValuePair<string, PackageVersion>> _versions = new();
            foreach (var version in package.versions)
            {
                _versions.Add(version);
            }
            return _versions;
        }
        /// <summary>
        /// 写入包版本
        /// </summary>
        /// <param name="package">包</param>
        /// <param name="version">版本</param>
        /// <param name="packageVersion">包版本</param>
        /// <returns>是否写入成功</returns>
        public static bool InsertVersion(this VPMDataPackage package, string version, PackageVersion packageVersion)
        {
            if (package.versions.ContainsKey(version))
            {
                return false;
            }
            var _versions = GetVersions(package);
            _versions.Insert(0, new KeyValuePair<string, PackageVersion>(version, packageVersion));
            package.versions.Clear();
            foreach (var _version in _versions)
            {
                package.versions.Add(_version.Key, _version.Value);
            }
            return true;
        }
        public static PackageVersion GetLatestVersion(this VPMDataPackage package)
        {
            PackageVersion latestVersion = null;
            foreach (var version in package.versions)
            {
                if (latestVersion == null)
                {
                    latestVersion = version.Value;
                    continue;
                }
                if (VersionCompare(version.Value.version, latestVersion.version) > 0)
                {
                    latestVersion = version.Value;
                }
            }
            return latestVersion;
        }
        /// <summary>
        /// 版本比较
        /// </summary>
        /// /// <param name="version1"></param>
        /// <param name="version2"></param>
        /// <returns></returns>
        static int VersionCompare(string version1, string version2)
        {
            version1 = version1.Trim();
            if (version1.ToLower().StartsWith("v"))
                version1 = version1[1..];
            version2 = version2.Trim();
            if (version2.ToLower().StartsWith("v"))
                version2 = version2[1..];
            var version1Array = version1.Split('.');
            var version2Array = version2.Split('.');
            for (int i = 0; i < version1Array.Length; i++)
            {
                if (version2Array.Length <= i)
                    return 1;
                // tryparse
                if (int.TryParse(version1Array[i], out var version1Int)
                && int.TryParse(version2Array[i], out var version2Int))
                {
                    if (version1Int > version2Int)
                        return 1;
                    else if (version1Int < version2Int)
                        return -1;
                }
                else
                {
                    if (string.Compare(version1Array[i], version2Array[i], StringComparison.Ordinal) > 0)
                        return 1;
                    else if (string.Compare(version1Array[i], version2Array[i], StringComparison.Ordinal) < 0)
                        return -1;
                }
            }
            if (version2Array.Length > version1Array.Length)
                return -1;
            return 0;
        }
    }
    /// <summary>
    /// 包版本
    /// </summary>
    public class PackageVersion
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name = string.Empty;
        /// <summary>
        /// 显示名称
        /// </summary>
        public string displayName = string.Empty;
        /// <summary>
        /// 版本号
        /// </summary>
        public string version = string.Empty;
        /// <summary>
        /// Unity 版本
        /// </summary>
        public string unity = string.Empty;
        /// <summary>
        /// 描述
        /// </summary>
        public string description = string.Empty;
        /// <summary>
        /// 文档地址
        /// </summary>
        public string documentationUrl = string.Empty;
        /// <summary>
        /// 更新日志地址
        /// </summary>
        public string changelogUrl = string.Empty;
        /// <summary>
        /// 许可地址
        /// </summary>
        public string licensesUrl = string.Empty;
        /// <summary>
        /// 许可
        /// </summary>
        public string license = string.Empty;
        /// <summary>
        /// 许可
        /// </summary>
        public string[] keywords = new string[0];
        /// <summary>
        /// 依赖
        /// </summary>
        /// <typeparam name="string">名称</typeparam>
        /// <typeparam name="string">版本号</typeparam>
        /// <returns>版本号</returns>
        public Dictionary<string, string> vpmDependencies = new();
        /// <summary>
        /// 示例
        /// </summary>
        public VersionSamples[] samples = new VersionSamples[0];
        /// <summary>
        /// 作者信息
        /// </summary>
        public VersionAuthor author = new();
        /// <summary>
        /// ZIP SHA256
        /// </summary>
        public string zipSHA256 = string.Empty;
        /// <summary>
        /// ZIP 下载地址
        /// </summary>
        public string url = string.Empty;
        /// <summary>
        /// 仓库地址
        /// </summary>
        public string repo = string.Empty;
        /// <summary>
        /// 旧版文件夹
        /// </summary>
        /// <typeparam name="string">文件夹路径</typeparam>
        /// <typeparam name="string">guid（在文件夹附属的.meta文件）</typeparam>
        /// <returns>guid（在文件夹附属的.meta文件）</returns>
        public Dictionary<string, string> legacyFolders = new();
    }
    /// <summary>
    /// 示例
    /// </summary>
    public class VersionSamples
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string displayName = string.Empty;
        /// <summary>
        /// 描述
        /// </summary>
        public string description = string.Empty;
        /// <summary>
        /// 路径
        /// </summary>
        public string path = string.Empty;
    }
    /// <summary>
    /// 作者信息
    /// </summary>
    public class VersionAuthor
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name = string.Empty;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email = string.Empty;
        /// <summary>
        /// 地址
        /// </summary>
        public string url = string.Empty;
    }
}

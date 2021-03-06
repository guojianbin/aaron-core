﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aaron.Core.Domain.Common;

namespace Aaron.Core.Web.UI
{
    public class PageTitleBuilder : IPageTitleBuilder
    {
        private readonly SeoSettings _seoSettings;
        private readonly List<string> _titleParts;
        private readonly List<string> _metaDescriptionParts;
        private readonly List<string> _metaKeywordParts;
        private readonly Dictionary<ResourceLocation, List<string>> _scriptParts;
        private readonly Dictionary<ResourceLocation, List<string>> _cssParts;
        private readonly List<string> _canonicalUrlParts;

        public PageTitleBuilder(SeoSettings seoSettings)
        {
            this._seoSettings = seoSettings;
            this._titleParts = new List<string>();
            this._metaDescriptionParts = new List<string>();
            this._metaKeywordParts = new List<string>();
            this._scriptParts = new Dictionary<ResourceLocation, List<string>>();
            this._cssParts = new Dictionary<ResourceLocation, List<string>>();
            this._canonicalUrlParts = new List<string>();
        }

        public void AddTitleParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _titleParts.Add(part);
        }
        public void AppendTitleParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _titleParts.Insert(0, part);
        }
        public string GenerateTitle(bool addDefaultTitle)
        {
            string result = "";
            var specificTitle = string.Join(_seoSettings.PageTitleSeparator, _titleParts.AsEnumerable().Reverse().ToArray());
            if (!String.IsNullOrEmpty(specificTitle))
            {
                if (addDefaultTitle)
                {
                    //main name + page title
                    switch (_seoSettings.PageTitleSeoAdjustment)
                    {
                        case PageTitleSeoAdjustment.PagenameAfterWebname:
                            {
                                result = string.Join(_seoSettings.PageTitleSeparator, _seoSettings.DefaultTitle, specificTitle);
                            }
                            break;
                        case PageTitleSeoAdjustment.WebnameAfterPagename:
                        default:
                            {
                                result = string.Join(_seoSettings.PageTitleSeparator, specificTitle, _seoSettings.DefaultTitle);
                            }
                            break;

                    }
                }
                else
                {
                    //page title only
                    result = specificTitle;
                }
            }
            else
            {
                //main name only
                result = _seoSettings.DefaultTitle;
            }
            return result;
        }


        public void AddMetaDescriptionParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _metaDescriptionParts.Add(part);
        }
        public void AppendMetaDescriptionParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _metaDescriptionParts.Insert(0, part);
        }
        public string GenerateMetaDescription()
        {
            var metaDescription = string.Join(", ", _metaDescriptionParts.AsEnumerable().Reverse().ToArray());
            var result = !String.IsNullOrEmpty(metaDescription) ? metaDescription : _seoSettings.DefaultMetaDescription;
            return result;
        }


        public void AddMetaKeywordParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _metaKeywordParts.Add(part);
        }
        public void AppendMetaKeywordParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _metaKeywordParts.Insert(0, part);
        }
        public string GenerateMetaKeywords()
        {
            var metaKeyword = string.Join(", ", _metaKeywordParts.AsEnumerable().Reverse().ToArray());
            var result = !String.IsNullOrEmpty(metaKeyword) ? metaKeyword : _seoSettings.DefaultMetaKeywords;
            return result;
        }

        public void AddMetaSeo<T>(T seoModel) where T : SEOEntityModel
        {
            if (!(seoModel is SEOEntityModel)) return;
            if (seoModel == null) return;
            _metaDescriptionParts.Add(seoModel.MetaDescription);
            _metaKeywordParts.Add(seoModel.MetaKeywords);
        }

        public void AddScriptParts(ResourceLocation location, params string[] parts)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<string>());

            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _scriptParts[location].Add(part);
        }
        public void AppendScriptParts(ResourceLocation location, params string[] parts)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<string>());

            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _scriptParts[location].Insert(0, part);
        }
        public void AppendLastScriptParts(ResourceLocation location, params string[] parts)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<string>());

            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _scriptParts[location].Insert(_scriptParts[location].Count - 1, part);
        }
        public string GenerateScripts(ResourceLocation location)
        {
            if (!_scriptParts.ContainsKey(location) || _scriptParts[location] == null)
                return "";

            var result = new StringBuilder();
            //use only distinct rows
            foreach (var scriptPath in _scriptParts[location].Distinct())
            {
                result.AppendFormat("<script src=\"{0}\" type=\"text/javascript\"></script>", scriptPath);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }


        public void AddCssFileParts(ResourceLocation location, params string[] parts)
        {
            if (!_cssParts.ContainsKey(location))
                _cssParts.Add(location, new List<string>());

            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _cssParts[location].Add(part);
        }
        public void AppendCssFileParts(ResourceLocation location, params string[] parts)
        {
            if (!_cssParts.ContainsKey(location))
                _cssParts.Add(location, new List<string>());

            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _cssParts[location].Insert(0, part);
        }
        public string GenerateCssFiles(ResourceLocation location)
        {
            if (!_cssParts.ContainsKey(location) || _cssParts[location] == null)
                return "";

            var result = new StringBuilder();
            //use only distinct rows
            foreach (var cssPath in _cssParts[location].Distinct())
            {
                result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", cssPath);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }


        public void AddCanonicalUrlParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _canonicalUrlParts.Add(part);
        }
        public void AppendCanonicalUrlParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _canonicalUrlParts.Insert(0, part);
        }
        public string GenerateCanonicalUrls()
        {
            var result = new StringBuilder();
            foreach (var canonicalUrl in _canonicalUrlParts)
            {
                result.AppendFormat("<link rel=\"canonical\" href=\"{0}\" />", canonicalUrl);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }
    }
}

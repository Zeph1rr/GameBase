using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace BaseGame.Editor
{
    public class ReleaseToolWindow : EditorWindow
    {
        private string _yourName = "";
        private string _yourRepo = "";
        private string _projectPath;
        private string _outputLog = "";
        private string _errorMessage = "";
        private float _progress = 0f;

        [MenuItem("Tools/Auto Version")]
        public static void ShowWindow()
        {
            GetWindow<ReleaseToolWindow>("Auto Version");
        }

        private void OnEnable()
        {
            _projectPath = Directory.GetCurrentDirectory();
        }

        private void OnGUI()
        {
            GUILayout.Label("Настройки GitHub", EditorStyles.boldLabel);
            _yourName = EditorGUILayout.TextField("Your Name (GitHub)", _yourName);
            _yourRepo = EditorGUILayout.TextField("Your Repo", _yourRepo);

            EditorGUILayout.Space();

            if (GUILayout.Button("Создать package.json"))
            {
                CreatePackageJsonIfNotExists();
            }

            if (GUILayout.Button("Установить standard-version"))
            {
                InstallStandardVersion();
            }

            if (GUILayout.Button("Запустить релиз"))
            {
                RunRelease();
            }

            EditorGUILayout.Space();

            if (!string.IsNullOrEmpty(_errorMessage))
            {
                EditorGUILayout.HelpBox(_errorMessage, MessageType.Error);
            }

            GUILayout.Label("Лог выполнения:", EditorStyles.boldLabel);
            EditorGUILayout.TextArea(_outputLog, GUILayout.Height(200));

            EditorGUILayout.Space();
            Rect rect = GUILayoutUtility.GetRect(50, 20);
            EditorGUI.ProgressBar(rect, _progress, $"{Mathf.RoundToInt(_progress * 100)}%");
        }

        private void CreatePackageJsonIfNotExists()
        {
            string packageJsonPath = Path.Combine(_projectPath, "package.json");

            if (!File.Exists(packageJsonPath))
            {
                string packageName = $"@{_yourName}/{_yourRepo}";

                string json = $@"{{
    ""name"": ""{packageName}"",
    ""version"": ""1.0.0"",
    ""private"": true,
    ""scripts"": {{
    ""release"": ""standard-version""
    }},
    ""standard-version"": {{
    ""types"": [
      {{ ""type"": ""feat"", ""section"": ""✨ Features"" }},
      {{ ""type"": ""fix"", ""section"": ""🐛 Bug Fixes"" }},
      {{ ""type"": ""docs"", ""section"": ""📝 Documentation"" }},
      {{ ""type"": ""style"", ""section"": ""💄 Styles"" }},
      {{ ""type"": ""refactor"", ""section"": ""♻️ Code Refactoring"" }},
      {{ ""type"": ""perf"", ""section"": ""⚡ Performance Improvements"" }},
      {{ ""type"": ""test"", ""section"": ""✅ Tests"" }},
      {{ ""type"": ""chore"", ""section"": ""🔧 Chores"" }}
    ],
    ""commitUrlFormat"": ""https://github.com/{_yourName}/{_yourRepo}/commit/{{{{hash}}}}"",
    ""compareUrlFormat"": ""https://github.com/{_yourName}/{_yourRepo}/compare/{{{{previousTag}}}}...{{{{currentTag}}}}"",
    ""issueUrlFormat"": ""https://github.com/{_yourName}/{_yourRepo}/issues/{{{{id}}}}""
      }}
    }}";

                File.WriteAllText(packageJsonPath, json, Encoding.UTF8);
                AppendOutput($"Создан package.json с именем пакета: {packageName}");
            }
            else
            {
                AppendOutput("package.json уже существует, пропускаем создание.");
            }
        }

        private void InstallStandardVersion()
        {
            _progress = 0.1f;
            string npmPath = GetNpmPath();

            AppendOutput("Устанавливаем standard-version...");
            RunCommand(npmPath, "install --save-dev standard-version", () =>
            {
                _progress = 1f;
                AppendOutput("standard-version успешно установлен.");
            });
        }

        private void RunRelease()
        {
            _progress = 0.1f;
            string npmPath = GetNpmPath();

            AppendOutput("Запускаем релиз (standard-version)...");
            RunCommand(npmPath, "run release", () =>
            {
                _progress = 1f;
                AppendOutput("Релиз завершён.");
            });
        }

        private string GetNpmPath()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
                return @"C:\Program Files\nodejs\npm.cmd";
            return "npm";
        }

        private void RunCommand(string fileName, string arguments, Action onComplete)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    WorkingDirectory = _projectPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process process = new Process { StartInfo = psi };
                process.OutputDataReceived += (sender, e) => { if (!string.IsNullOrEmpty(e.Data)) AppendOutput(e.Data); };
                process.ErrorDataReceived += (sender, e) => { if (!string.IsNullOrEmpty(e.Data)) AppendError(e.Data); };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.EnableRaisingEvents = true;
                process.Exited += (sender, e) => onComplete?.Invoke();
            }
            catch (Exception ex)
            {
                AppendError("Исключение: " + ex.Message);
            }
        }

        private void AppendOutput(string message)
        {
            _outputLog += message + "\n";
            RepaintMainThread();
        }

        private void AppendError(string message)
        {
            _errorMessage = message;
            AppendOutput("ERROR: " + message);
            RepaintMainThread();
        }

        private void RepaintMainThread()
        {
            EditorApplication.delayCall += () =>
            {
                if (this != null)
                    Repaint();
            };
        }
    }
}

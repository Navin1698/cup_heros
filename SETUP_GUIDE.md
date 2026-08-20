# 📱 GitHub Actions Android Build Guide (No Unity Installation Needed)

This workflow builds your Unity Android `.apk` file directly on GitHub's cloud servers without needing Unity installed on your computer.

---

## 🚀 Step 1: Push your Code to GitHub

Open terminal in this directory and run:
```bash
git init
git add .
git commit -m "Add GitHub Actions workflow for Android APK build"
git branch -M main
git remote add origin https://github.com/<YOUR_USERNAME>/<YOUR_REPO_NAME>.git
git push -u origin main
```

---

## 🔑 Step 2: Set up Unity License Secret (One-Time Setup)

To allow GitHub Actions to build Unity projects for free, Unity requires a free Personal License (`UNITY_LICENSE`).

### Quick Way:
1. Go to your GitHub Repository -> **Actions** tab.
2. Select **"Get Unity Activation File (.alf)"** from the left sidebar and click **Run workflow**.
3. Once completed, download the `UnityActivationFile.alf` artifact.
4. Open [license.unity3d.com](https://license.unity3d.com) in your web browser.
5. Upload the `.alf` file and choose **Unity Personal (Free)**.
6. Download the resulting `.ulf` license file.
7. Open the `.ulf` file in Notepad and copy all its text content.

### Add Secret to GitHub:
1. Go to your GitHub Repository -> **Settings** -> **Secrets and variables** -> **Actions**.
2. Click **New repository secret**.
3. Name: `UNITY_LICENSE`
4. Value: Paste the text content of your `.ulf` file.
5. Click **Add secret**.

---

## 📦 Step 3: Run Build & Download APK

1. Go to your GitHub Repository -> **Actions** tab.
2. Click **"Build Android APK"** -> **Run workflow**.
3. GitHub's cloud servers will build your APK (takes ~5-8 minutes).
4. When finished, click on the completed run to download **`Game1-Android-APK`**.
5. Transfer the `.apk` file to your mobile phone and install it!

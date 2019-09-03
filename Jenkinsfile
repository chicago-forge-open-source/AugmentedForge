pipeline {
    agent {
        docker { image 'gableroux/unity3d:2019.1.14f1-android' }
    }
    environment {
        UNITY_LICENSE_1_14_ANDROID = credentials('UNITY_LICENSE_1_14_ANDROID')
        SERVICE_ACCOUNT_JSON = credentials('SERVICE_ACCOUNT_JSON')
        KEYSTORE_PASS = 'pillar4life'
        ANDROID_SDK_ROOT = '/opt/android-sdk-linux'
        ANDROID_NDK_HOME = '/android-ndk-r16b'
    }
    stages {
        stage('Deploy') {
            steps {
//                 sh '''#!/bin/bash
//                     apt-get update
//                     apt-get install -y git
//                     rm -rf AugmentedForge
//                     git clone https://github.com/chicago-forge-open-source/AugmentedForge
//                 '''
                sh '''#!/bin/bash             
                    wget -q https://dl.google.com/android/repository/android-ndk-r16b-linux-x86_64.zip
                    unzip -q android-ndk-r16b-linux-x86_64.zip -d /
                '''        
                sh '''#!/bin/bash
                    cd AugmentedForge/
                    mkdir deploy/secrets
                    echo $SERVICE_ACCOUNT_JSON | base64 --decode > deploy/secrets/serviceAccount.json
                    echo $UNITY_LICENSE_1_14_ANDROID | base64 --decode >> .circleci/Unity_License.ulf
                    cp -f ./Packages/manifest-linux.json ./Packages/manifest.json
                    /opt/Unity/Editor/Unity -quit -batchmode -nographics -silent-crashes -logFile -manualLicenseFile .circleci/Unity_License.ulf
                    touch /root/.android/repositories.cfg
                '''                    
                sh '''#!/bin/bash
                    /opt/Unity/Editor/Unity -batchmode -nographics -silent-crashes -quit -executeMethod Editor.Export.ExportAndroidAab
                    cd deploy && ./gradlew publishBundle --scan --artifact-dir $(pwd)/../android-output
                '''
            }
        }
    }
}

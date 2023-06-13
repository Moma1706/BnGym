import React, { useState, useEffect, useCallback } from 'react';
import { StyleSheet, Alert, View, Text, Dimensions } from 'react-native';
import { Camera, useCameraDevices } from 'react-native-vision-camera';
import Layout from './Layout';
import { NavigationProp, ParamListBase } from '@react-navigation/native';
import { BarcodeFormat, useScanBarcodes } from 'vision-camera-code-scanner';
import { RNHoleView } from 'react-native-hole-view';
import { PERMISSIONS, request, RESULTS } from 'react-native-permissions';
import { Platform } from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';
import axios from 'axios';
import { BASE_URL } from '../config/api-url.config';

type CheckInProps = {
  navigation: NavigationProp<ParamListBase>;
};

const Constants = {
  CHECK_IN_MESSAGE: 'Čekiranje uspiješno završeno!',
  CHECK_IN_MESSAGE_FAILED: 'QR kod nije validan. Molimo Vas, pokušajte ponovo.',
  CHECK_IN_PROCESS: 'check in process',

  VIEW_HEIGHT: Dimensions.get('window').height - 180,
  RN_HOLE_HEIGHT: 500,
  RN_HOLE_WIDTH: 300
}

const CheckIn = ({ navigation }: CheckInProps) => {
  const checkInUrl = `${BASE_URL}/CheckIn`;
  const devices = useCameraDevices('wide-angle-camera');
  const device = devices.back;

  const [isCodeInvalid, setIsCodeInvalid] = useState<any>(null);
  const [isPermissionGranted, setIsPermissionGranted] = useState<any>(false);
  const [isRequestSent, setIsRequestSent] = useState<any>(false);

  const [frameProcessor, barcodes] = useScanBarcodes([
    BarcodeFormat.QR_CODE,
  ]);
  const checkCameraPermission =  useCallback(() => {
    request(Platform.OS === 'ios' ? PERMISSIONS.IOS.CAMERA : PERMISSIONS.ANDROID.CAMERA).then((result) => {
        if (result === RESULTS.GRANTED) {
          setIsPermissionGranted(true)
        } else {
          setIsPermissionGranted(false)
        }
    });
  }, [])
  
  useEffect(() => {
    checkCameraPermission();
  }, [checkCameraPermission]);

  useEffect(() => {
    toggleActiveState();
    return () => {
      barcodes;
    };
  }, [barcodes]);

  const showAlert = (message: string) => {
    Alert.alert(
      message,
      'Da biste ponovo skenirali pritisnite OK',
      [
        {
          text: 'Nazad',
          onPress: () => {
            setIsCodeInvalid(null);
            navigation.navigate('Profile');
          },
          style: 'cancel',
        },
        {
          text: 'OK',
          onPress: () => {
            setIsCodeInvalid(null);
          },
        },
      ],
      { cancelable: false }
    );
  };

const toggleActiveState = async () => {
  if (isCodeInvalid) {
    return;
  }

    if (barcodes && barcodes.length > 0) {
      console.log('je l udjes tuuuu')
      barcodes.forEach(async (scannedBarcode: any) => {
        if (scannedBarcode.rawValue !== '' && scannedBarcode.rawValue === Constants.CHECK_IN_PROCESS) {
          if (!isRequestSent) {
            console.log('if not sent')
            setIsRequestSent(true);
            try {
              const gymUserId = await EncryptedStorage.getItem('gym_user_id');
              const response = await axios.post(checkInUrl, JSON.stringify({
                GymUserId: gymUserId
              }), { headers: {"content-type": "application/json" }});
  
              if (response.status === 200) {
                navigation.navigate('Profile');
                Alert.alert(Constants.CHECK_IN_MESSAGE);
              } else {
                Alert.alert(response.data.message);
              }
              setIsRequestSent(false);
            } catch (error: any) {
              setIsRequestSent(false);
              navigation.navigate('Profile');
              if (error.response && (error.response.status === 400))
                Alert.alert(error.response.data.message);
              else
                Alert.alert("Došlo je do greške prilikom čekiranja");
            }
          } else {
            console.log('ipak ovaj')
          }
        
        } else {
          console.log('invalid else...')
          setIsCodeInvalid(true);
          showAlert(Constants.CHECK_IN_MESSAGE_FAILED)
        }
      });
    }
  };

  return (
    <Layout navigation={navigation}>
    {
    (device == null || !isPermissionGranted) ?
    ( 
      <View style={styles.container}>
        <Text>Go to setting and allow camera</Text>
      </View>
    ) :
    <>
      <View style={styles.container}>
        <Camera
          style={StyleSheet.absoluteFill}
          device={device}
          isActive={true} 
          frameProcessor={frameProcessor}
          frameProcessorFps={5}
          audio={false}
        />
       <RNHoleView
          holes={[
            {
              x:  Dimensions.get('window').width / 2 - Constants.RN_HOLE_WIDTH / 2,
              y: Constants.VIEW_HEIGHT / 2 - Constants.RN_HOLE_HEIGHT / 2,
              width: Constants.RN_HOLE_WIDTH,
              height: Constants.RN_HOLE_HEIGHT,
              borderRadius: 10,
             
            },
          ]}
          style={styles.rnholeView}
        />
      </View>
    </>
    }
    </Layout>
  );
}

const styles = StyleSheet.create({
  container: {
    height: Constants.VIEW_HEIGHT,
  },
  rnholeView: {
    position: 'absolute',
    width: '100%',
    height: '100%',
    alignSelf: 'center',
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: 'rgba(0,0,0,0.5)',
   
  },
});

export default CheckIn;

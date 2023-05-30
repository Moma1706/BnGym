import React, { useState, useEffect, useRef } from 'react';
import { StyleSheet, Alert, ActivityIndicator, View, Text } from 'react-native';
import { Camera, useCameraDevices, useFrameProcessor } from 'react-native-vision-camera';
import Layout from './Layout';
import { NavigationProp, ParamListBase } from '@react-navigation/native';
import axios from "axios";

type CheckInProps = {
  navigation: NavigationProp<ParamListBase>;
};

const CheckIn = ({ navigation }: CheckInProps) => {
  const camera = useRef<Camera>(null);
  const devices = useCameraDevices('wide-angle-camera');
  const device = devices.back;


  // const frameProcessor = useFrameProcessor((frame) => {
  //   console.log(frame);
  //   const scannedQRCode = frame.toString();
  //   const expectedQRCode = 'check-in-process'; // Replace with your expected QR code
  //   if (scannedQRCode !== expectedQRCode) {
  //     Alert.alert('Error', 'Invalid QR code.');
  //     return;
  //   }

  //   Alert.alert('Dobar code');
    // const gymUserId = 'YOUR_ENCRYPTED_STORAGE_ID';
    // const url = `/api/App/check-in/${gymUserId}`;
    // fetch(url, {
    //   method: 'POST',
    // })
    //   .then(response => response.json())
    //   .then(data => {
    //     Alert.alert('Success', 'Successfully checked in.');
    //   })
    //   .catch(error => {
    //     Alert.alert('Error', 'An error occurred while checking in.');
    //   });
  // }, [])
  const frameProcessor = useFrameProcessor((frame) => {
    'worklet'
    console.log(frame);
    // const qrCodes = scanQRCodes(frame)
    // if (qrCodes.length > 0) {
    //   runOnJS(onQRCodeDetected)(qrCodes[0])
    // }
  }, [])

  if (device == null)
    return ( 
    // <Layout navigation={navigation}>
    //   <ActivityIndicator size='large'/>
    // </Layout>
    <View style={styles.box1}/>
    );

  return (
    <Layout navigation={navigation}>
      <View style={styles.box}>
      <Camera
          style={StyleSheet.absoluteFill}
          device={device}
          isActive={true}
          frameProcessor={frameProcessor}
          // frameProcessorFps={5}
          video = {true}
          audio = {true}
        />
      </View>
    </Layout>
  );
}

const styles = StyleSheet.create({
  box: {
    width: '100%',
    height: '100%',
  },

  box1: {
    width: 300,
    height: 200,
    backgroundColor: 'green'
  },
  barcodeTextURL: {
    fontSize: 20,
    color: 'white',
    fontWeight: 'bold',
  },
  button: {
    backgroundColor: 'blue',
    padding: 10,
    borderRadius: 5,
    alignItems: 'center',
  },
  buttonText: {
    color: 'white',
    fontWeight: 'bold',
  },
  camera: {
    flex: 1,
  },
});

export default CheckIn;

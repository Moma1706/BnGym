import { ActivityIndicator } from '@react-native-material/core';
import React from 'react';
import {StyleSheet, View, Dimensions } from 'react-native';

type CustomSpinnerProps = {
    visible: boolean;
};

const CustomSpinner = ({ visible }: CustomSpinnerProps) => {
  return (
    <View style={[styles.container, !visible && styles.hidden]}>
      <ActivityIndicator size='large' color='white'/>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    height: Dimensions.get('window').height,
    width:  Dimensions.get('window').width,
    position: 'absolute',
    backgroundColor: 'rgba(0, 0, 0, 0.7)'
  },

  hidden: {
    display: 'none'
  }

});

export default CustomSpinner;

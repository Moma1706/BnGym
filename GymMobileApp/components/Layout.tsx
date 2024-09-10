import React, { ReactNode } from 'react';
import { View, Text, TouchableOpacity, StyleSheet, Dimensions } from 'react-native';
import { NavigationProp, ParamListBase } from '@react-navigation/native';

type LayoutProps = { 
  navigation: NavigationProp<ParamListBase>;
  children: ReactNode
};

const Layout =  ({ navigation, children }: LayoutProps) => {
  const handleCheckInPress = () => {
      navigation.navigate('CheckIn');
    };
  
    const handleMyProfilePress = () => {
      navigation.navigate('Profile');
    };

  return (
    <View style={styles.container}>
      <View style={styles.header}/>
      {children}
      <View style={styles.buttonsContainer}>
        <TouchableOpacity style={styles.button} onPress={handleCheckInPress}>
          <Text style={styles.buttonText}>Čekiraj se</Text>
        </TouchableOpacity>
        <TouchableOpacity style={styles.button} onPress={handleMyProfilePress}>
          <Text style={styles.buttonText}>Profil</Text>
        </TouchableOpacity>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    backgroundColor: 'white',
    height: Dimensions.get('window').height,
  },

  header: {
    backgroundColor: '#18232e',
    height: 80
  },

  buttonsContainer: {
    flexDirection: 'row',
    height: 100,
    width: '100%',
    position: "absolute",
    bottom: 0
  },
  button: {
    flex: 1,
    backgroundColor: '#2c3e50',
    alignItems: 'center',
    justifyContent: 'center'
    //padding: 15
  },
  buttonText: {
    color: '#fff',
    fontSize: 20,
    textAlign: "center"
  }
});

export default Layout;

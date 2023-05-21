import React, { ReactNode } from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import { NavigationProp, ParamListBase } from '@react-navigation/native';
import { Image } from 'react-native-elements';
// import { Icon } from 'react-native-elements'

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
    flex: 1,
    backgroundColor: '#F5FCFF',
  },
  buttonsContainer: {
    flexDirection: 'row',
    height: '11%',
    width: '100%',
  },
  button: {
    flex: 1,
    backgroundColor: '#2c3e50',
    alignItems: 'center',
    padding: 15
  },
  buttonText: {
    color: '#fff',
    fontSize: 20,
    textAlign: "center"
  },
  imageContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    marginBottom: 20,
  },
  image: {
    width: 50,
    height: 50,
    marginRight: 10,
  },
});

export default Layout;

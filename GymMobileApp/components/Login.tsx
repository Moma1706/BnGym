import React, { useState, useEffect } from 'react';
import { Image, Alert, StyleSheet, Text, TextInput, TouchableOpacity, View, Dimensions } from 'react-native';
import axios from "axios";
import { NavigationProp, ParamListBase } from '@react-navigation/native';
import EncryptedStorage from 'react-native-encrypted-storage';
import { KeyboardAwareScrollView } from 'react-native-keyboard-aware-scroll-view';
import { BASE_URL } from '../config/api-url.config';
import CustomSpinner from './CustomSpinner';

type LoginProps = {
  navigation: NavigationProp<ParamListBase>;
};

const Login = ({ navigation }: LoginProps) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const loginUrl = `${BASE_URL}/Auth/login-app`;


  useEffect(() => {
    // if user_id present in local storage, it means that user is login and show profile screen. Otherwise show login screen
    EncryptedStorage.getItem('user_id')
      .then(userId => {
        if (userId)
          navigation.navigate('Profile');
        else
          navigation.navigate('Login');
      })
      .catch(error => {
        console.log(error);
      });
  });

  const handleLogin = async () => {
    if (email.trim() === "" || password.trim() === "") {
      // One of the fields is empty
      Alert.alert("Email i lozinka ne mogu biti prazni");
      return;
    }
  
    const emailRegex = /\S+@\S+\.\S+/;
    if (!emailRegex.test(email)) {
      // Invalid email format
      Alert.alert("Email nije validan");
      return;
    }

    try {
      // send API request
      setIsLoading(true);
      const response = await axios.post(loginUrl, JSON.stringify({
        Email: email,
        Password: password
      }), { headers: {"content-type": "application/json" }});

      // API returns 200 OK
      if (response.status === 200) {
        setIsLoading(false);
        // reset email and password values
        setPassword('');
        setEmail('');

        // save ids to local storage
        await EncryptedStorage.setItem(
          "gym_user_id", response.data.gymUserId
        );
        await EncryptedStorage.setItem(
          "user_id", response.data.userId.toString()
        );

        // navigate to profile screen
        navigation.navigate('Profile');
      } else {
        setIsLoading(false);
        Alert.alert(response.data.message);
      }

    } catch (error: any) {
      setIsLoading(false);
      if (error.response && (error.response.status === 400 || error.response.status === 401))
        Alert.alert(error.response.data.error);
      else
        Alert.alert("Došlo je do greške prilikom prijavljivanja");
    }
  };

  return (
    <KeyboardAwareScrollView enableOnAndroid={true}>
    <View style={styles.container}>
      <Image source={require('../assets/logo.png')} style={styles.logo} />
      <TextInput
        style={styles.input}
        placeholder="Email"
        placeholderTextColor={"black"}
        value={email}
        keyboardType='email-address'
        autoCapitalize='none'
        onChangeText={text => setEmail(text)}
      />
      <TextInput
        style={styles.input}
        placeholder="Lozinka"
        placeholderTextColor={"black"}
        value={password}
        onChangeText={text => setPassword(text)}
        secureTextEntry={true}
      />
      <TouchableOpacity style={styles.button} onPress={handleLogin}>
        <Text style={styles.buttonText}>Prijavi se</Text>
      </TouchableOpacity>
      <CustomSpinner visible={isLoading}/>
    </View>
    </KeyboardAwareScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    height: Dimensions.get('window').height,
  },
  logo: {
    width: 350,
    height: 350,
    borderRadius: 300
  },
  input: {
    width: '80%',
    height: 50,
    borderWidth: 1,
    borderColor: '#ccc',
    borderRadius: 5,
    paddingHorizontal: 10,
    marginVertical: 10,
    color: 'black'
  },
  button: {
    width: '80%',
    height: 50,
    backgroundColor: '#2c3e50',
    borderRadius: 5,
    alignItems: 'center',
    justifyContent: 'center',
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
  }
});

export default Login;

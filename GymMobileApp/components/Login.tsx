import React, { useState, useEffect } from 'react';
import { Image, Alert, StyleSheet, Text, TextInput, TouchableOpacity, View } from 'react-native';
import axios from "axios";
import { NavigationProp, ParamListBase } from '@react-navigation/native';
import EncryptedStorage from 'react-native-encrypted-storage';
import { KeyboardAwareScrollView } from 'react-native-keyboard-aware-scroll-view';

type LoginProps = {
  navigation: NavigationProp<ParamListBase>;
};

const Login = ({ navigation }: LoginProps) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [secureTextEntry, setSecureTextEntry] = useState(true);

  useEffect(() => {
    // if user_id present in local storage, it means that user is login and show profile screen. Otherwise shoe login screen
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

  const toggleSecureEntry = () => {
    setSecureTextEntry(!secureTextEntry);
  };

  const handleLogin = async () => {
    // navigate to profile screen
    navigation.navigate('Profile');
    
    // if (email.trim() === "" || password.trim() === "") {
    //   // One of the fields is empty
    //   Alert.alert("Email i lozinka ne mogu biti prazni");
    //   return;
    // }
  
    // const emailRegex = /\S+@\S+\.\S+/;
    // if (!emailRegex.test(email)) {
    //   // Invalid email format
    //   Alert.alert("Email nije validan");
    //   return;
    // }

    // try {
    //   // send API request
    //   // za android: 
    //   // const response = await axios.post(`http://10.0.2.2:42068/api/Auth/login-app`, JSON.stringify({
    //   const response = await axios.post(`http://localhost:42068/api/Auth/login-app`, JSON.stringify({
    //     Email: email,
    //     Password: password
    //   }), { headers: {"content-type": "application/json" }});

    //   // API returns 200 OK
    //   if (response.status === 200) {
    //     // reset email and password values
    //     setPassword('');
    //     setEmail('');

    //     // save ids to local storage
    //     await EncryptedStorage.setItem(
    //       "gym_user_id", response.data.gymUserId
    //     );
    //     await EncryptedStorage.setItem(
    //       "user_id", response.data.userId.toString()
    //     );

    //     // navigate to profile screen
    //     navigation.navigate('Profile');
    //   } else
    //     Alert.alert(response.data.message);

    // } catch (error: any) {
    //   if (error.response && (error.response.status === 400 || error.response.status === 401))
    //     Alert.alert(error.response.data.error);
    //   else
    //     Alert.alert("Došlo je do greške prilikom prijavljivanja");
    // }
  };

  return (
    // <KeyboardAwareScrollView>
    <View style={styles.container}>
      <Image source={require('../assets/black_image.jpeg')} style={styles.logo} />
      <TextInput
        style={styles.input}
        placeholder="Email"
        value={email}
        onChangeText={text => setEmail(text)}
      />
      <TextInput
        style={styles.input}
        placeholder="Lozinka"
        value={password}
        onChangeText={text => setPassword(text)}
        secureTextEntry={true}
      />
      <TouchableOpacity style={styles.button} onPress={handleLogin}>
        <Text style={styles.buttonText}>Prijavi se</Text>
      </TouchableOpacity>
    </View>
    // </KeyboardAwareScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  logo: {
    width: '80%',
    height: 300,
    borderRadius: 200
  },
  input: {
    width: '80%',
    height: 50,
    borderWidth: 1,
    borderColor: '#ccc',
    borderRadius: 5,
    paddingHorizontal: 10,
    marginVertical: 10,
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

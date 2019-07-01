const functions = require('firebase-functions');

const admin = require('firebase-admin');
admin.initializeApp();

exports.updateUser = functions.firestore
  .document('predict/{happen}')
  .onCreate(async (record, context) => {
        console.log('onUpdate');
        if(!record.exists)
              return;
        const newValue = record.data();
        const happen = newValue.happen;
        const when = newValue.when;
        const payload = {
            notification: {
                title: 'お告げがありました',
                body: when + ' ' + happen
            }
        };
        const refs = await admin.firestore().collection('token').listDocuments();
        const index = Math.floor(Math.random() * Math.floor(refs.length - 1));
        const snapshot = await refs[index].get();
        const deviceToken = snapshot.get('token');
        console.log('Send FCM:' + deviceToken );
        const response = await admin.messaging().sendToDevice(deviceToken, payload);
        console.log('Send FCM Done:' + response.successCount);
        record.ref.delete().then(function() {
            console.log("Document successfully deleted!");
        }).catch(function(error) {
            console.error("Error removing document: ", error);
        });
        return;
  });
db.getCollectionNames().forEach(function(collection) {
    db.getCollection(collection)
        .updateMany({}, { 
            $set: { UserId: NUUID('00000000-0000-0000-0000-000000000000') } 
        });
});